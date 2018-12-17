using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public abstract class HubRequestBase<TDto> : HubRequestFilter, IHubRequest<TDto>
    {
        private static Func<TDto, bool> _defaultFilter = (dto) => true;
        private readonly Func<TDto, bool> _filter;

        public HubRequestBase(Expression<Func<TDto, bool>> filter): base(filter)
        {
            _filter = filter == null ? _defaultFilter : filter.Compile();
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
