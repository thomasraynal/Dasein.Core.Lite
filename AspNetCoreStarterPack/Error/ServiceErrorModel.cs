using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AspNetCoreStarterPack.Error
{
    public class ServiceErrorModel
    {
        public string Reason { get; set; }
        public string Details { get; set; }
    }   
}
