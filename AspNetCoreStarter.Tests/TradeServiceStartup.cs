using AspNetCoreStarter.Demo.Common.Domain;
using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarter.Tests.Infrastructure;
using AspNetCoreStarter.Tests.Module;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Cache;
using AspNetCoreStarterPack.Default;
using AspNetCoreStarterPack.Extensions;
using AspNetCoreStarterPack.GraphQL;
using AspNetCoreStarterPack.Infrastructure;
using AspNetCoreStarterPack.SignalR;
using GraphQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StructureMap;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests
{
    public class TradeServiceRegistry : Registry
    {
        public TradeServiceRegistry()
        {
            Scan(scanner =>
            {
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
                scanner.WithDefaultConventions();
                scanner.AddAllTypesOf<IPublisher>();
                this.RegisterCacheProxy<ITradeService, TradeService>();
                this.RegisterCacheProxy<IPriceService, PriceService>();

            });
        }
    }

    public class TradeServiceStartup : ServiceStartupBase<TradeServiceConfiguration>
    {
        public TradeServiceStartup(IHostingEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
        }

        protected override void ConfigureServicesInternal(IServiceCollection services)
        {
            services.AddSerilog(Configuration);
            services.AddSingleton<IHubContextHolder<Price>,HubContextHolder<Price>>();
            services.AddSingleton<ICacheStrategy<MethodCacheObject>, DefaultCacheStrategy<MethodCacheObject>>();
            services.AddTransient<IAuthorizationHandler, ClaimRequirementHandler>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ServiceConfiguration.Key));

                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            IssuerSigningKey = secret,
                            ValidIssuer = ServiceConfiguration.Name,
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidateActor = false,
                            ValidateAudience = false
                        };

                        options.Events = new JwtBearerEvents()
                        {
                            OnAuthenticationFailed = context =>
                            {
                                throw new UnauthorizedUserException($"Failed to authenticate user [{context.Exception.Message}]");
                            }
                        };
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(TradeReferential.TraderUserPolicy, policy => policy.Requirements.Add(new ClaimRequirement(ClaimTypes.Role, TradeReferential.TraderClaimValue)));
                options.AddPolicy(TradeReferential.EquityTraderUserPolicy, policy => policy.Requirements.Add(new ClaimRequirement(ClaimTypes.Role, TradeReferential.EquityTraderClaimValue)));
            });

            var jsonSettings = new ServiceJsonSerializerSettings();

            services
                    .AddSignalR(hubOptions =>
                    {
                        hubOptions.EnableDetailedErrors = true;
                        hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
                    })
                    .AddJsonProtocol(options =>
                    {
                        options.PayloadSerializerSettings = jsonSettings;
                    });

            services.AddMvc()
                    .RegisterJsonSettings(jsonSettings);

        }

        protected override void OnApplicationStart()
        {
            Task.Delay(500).ContinueWith(async (_) =>
            {
                var publishers = AppCore.Instance.GetAll<IPublisher>();

                foreach(var publisher in publishers)
                {
                    await publisher.Start();
                }
                
            });
        }

        protected override void ConfigureInternal(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseMiddleware<GraphQLMiddleware<TradeServiceQuery,ITrade>>();
            
            app.UseSignalR(routes =>
            {
                routes.MapHub<PriceHub>("");
            });

            app.UseMvc();
        }
    }
}
