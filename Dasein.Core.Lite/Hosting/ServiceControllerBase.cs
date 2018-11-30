using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ServiceControllerBase : ControllerBase
    {
    }

    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ServiceControllerBase<TService> : ControllerBase
    {
        public TService Service
        {
            get
            {
                return AppCore.Instance.GetService<TService>();
            }
        }
    }
}
