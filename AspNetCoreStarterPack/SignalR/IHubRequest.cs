﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.SignalR
{
    public interface IHubRequest<TDto> : IHubRequestFilter
    {
       Func<TDto,bool> Filter { get; }
    }
}
