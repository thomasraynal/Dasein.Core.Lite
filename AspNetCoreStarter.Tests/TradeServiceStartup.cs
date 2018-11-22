using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarter.Tests.Infrastructure;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Cache;
using AspNetCoreStarterPack.Default;
using AspNetCoreStarterPack.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StructureMap;
using System.Security.Claims;
using System.Text;

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
            //services.AddSingleton<ITradeService, TradeService>();
            services.AddSingleton<ICacheStrategy<MethodCacheObject>, DefaultCacheStrategy<MethodCacheObject>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ServiceConfiguration.Key));

                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            IssuerSigningKey = secret,
                            ValidIssuer = ServiceConfiguration.Name,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateActor = false
                        };

                        //options.Events = new JwtBearerEvents()
                        //{
                        //    OnAuthenticationFailed = (context) =>
                        //    {
                        //        throw new UnauthorizedUserException();
                        //    }
                        //};

                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(TradeReferential.TraderUserPolicy, policy => policy.Requirements.Add(new ClaimRequirement(ClaimTypes.Role, TradeReferential.TraderClaimValue)));
                options.AddPolicy(TradeReferential.EquityTraderUserPolicy, policy => policy.Requirements.Add(new ClaimRequirement(ClaimTypes.Role, TradeReferential.EquityTraderClaimValue)));
            });

            services.AddMvc()
                    .AddJsonSettings(new ServiceJsonSerializerSettings());

        }

        protected override void ConfigureInternal(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
