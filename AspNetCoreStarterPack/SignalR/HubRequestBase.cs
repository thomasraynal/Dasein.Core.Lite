﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspNetCoreStarterPack.SignalR
{
    public abstract class HubRequestBase<TDto> : HubRequestFilter, IHubRequest<TDto>
    {
        private Func<TDto, bool> _filter;

        public HubRequestBase(Expression<Func<TDto, bool>> filter): base(filter)
        {
            _filter = filter.Compile();
        }

        public Func<TDto, bool> Filter
        {
            get
            {
                return _filter;
            }
        }
    }
}
