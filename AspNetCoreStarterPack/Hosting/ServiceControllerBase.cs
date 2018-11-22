using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ServiceControllerBase : ControllerBase
    {
    }
}
