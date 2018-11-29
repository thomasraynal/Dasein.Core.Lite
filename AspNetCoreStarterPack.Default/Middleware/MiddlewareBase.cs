using AspNetCoreStarterPack.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.Default
{
    public abstract class MiddlewareBase<TService> : IMiddleware<TService> 
    {
        private TService _service;
        public TService Service
        {
            get
            {
                if (null != _service) return _service;
                _service = AppCore.Instance.Get<TService>();
                if (null == _service) throw new MissingServiceException($"Service [{ typeof(TService)}] is not implemented");
                return _service;
            }
        }
    }
}
