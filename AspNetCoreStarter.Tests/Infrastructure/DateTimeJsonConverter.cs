using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Infrastructure
{
    public class DateTimeJsonConverter : IsoDateTimeConverter
    {
        public DateTimeJsonConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddTHH:mm";
        }
    }

   
}
