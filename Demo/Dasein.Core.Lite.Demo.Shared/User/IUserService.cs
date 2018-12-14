﻿using Refit;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Shared
{
    public interface IUserService
    {
        [Post("/api/v1/auth")]
        Task<TradeServiceToken> Login(UserCredentials credentials);
    }
}