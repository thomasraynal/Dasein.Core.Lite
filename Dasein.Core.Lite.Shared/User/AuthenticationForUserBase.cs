using Dasein.Core.Lite;
using Dasein.Core.Lite.Shared;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Service.Authentication
{
    public abstract class AuthenticationForUserBase : AuthenticationServiceBase, IService<IAuthenticationForUser>, IAuthenticationForUser
    {
        protected abstract Task<IServiceUserToken> AuthenticateUserInternal(Credentials credentials);

        public async Task<IServiceUserToken> AuthenticateUser(Credentials credentials)
        {
            var user = await AuthenticateUserInternal(credentials);
            user.AddClaim(new Claim(ServiceClaimConstants.Expiration, TokenExpiration.ToString()));


            var now = DateTime.Now;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ServiceConfiguration.Key));
            var issuer = ServiceConfiguration.Name;
            var handler = new JwtSecurityTokenHandler();
            var signingCreds = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
            var token = handler.CreateJwtSecurityToken(issuer, null, user as ClaimsIdentity, now, DateTime.Now.AddDays(1), now, signingCreds);
            var encoded = handler.WriteToken(token);

            user.AddClaim(new Claim(ServiceClaimConstants.Digest, encoded));

            return user;
        }
    }
}
