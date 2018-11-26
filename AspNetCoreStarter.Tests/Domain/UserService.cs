using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreStarter.Tests.Infrastructure;
using AspNetCoreStarterPack.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreStarter.Tests.Domain
{
    public class UserService : IUserService
    {
        private TradeServiceConfiguration _serviceConfiguration;

        public UserService(TradeServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
        }

        private ClaimsIdentity GetUser(CredentialsDto credentials)
        {
            var user = new ClaimsIdentity();

            var claimValue = credentials.Username == TradeReferential.EquityTraderClaimValue ? TradeReferential.EquityTraderClaimValue : TradeReferential.TraderClaimValue;

            user.AddClaim(new Claim(ClaimTypes.Role, claimValue));

            return user;
        }

        public Task<TradeServiceToken> Login(CredentialsDto credentials)
        {
            var now = DateTime.Now;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_serviceConfiguration.Key));
            var issuer = _serviceConfiguration.Name;
            var identity = GetUser(credentials);
            var handler = new JwtSecurityTokenHandler();
            var signingCreds = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
            var token = handler.CreateJwtSecurityToken(issuer, null, identity, now, DateTime.Now.AddDays(1), now, signingCreds);
            var encoded = handler.WriteToken(token);

            var result = new TradeServiceToken()
            {
                Username = credentials.Username,
                Digest = encoded
            };

            return Task.FromResult(result);

        }
    }
}
