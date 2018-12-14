﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dasein.Core.Lite.Demo.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Dasein.Core.Lite.Demo.Server
{
    public class UserService : IUserService
    {
        private TradeServiceConfiguration _serviceConfiguration;

        public UserService(TradeServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
        }

        private ClaimsIdentity GetUser(UserCredentials credentials)
        {
            var user = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            user.AddClaim(new Claim(ClaimTypes.Name, credentials.Username));
            user.AddClaim(new Claim(ClaimTypes.Role, TradeServiceReferential.TraderClaimValue));

            if (credentials.Username == TradeServiceReferential.EquityTraderClaimValue)
            {
                user.AddClaim(new Claim(ClaimTypes.Role, TradeServiceReferential.EquityTraderClaimValue));
            }

            return user;
        }

        public Task<TradeServiceToken> Login(UserCredentials credentials)
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
