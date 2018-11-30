using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class DateTimeJsonConverter : IsoDateTimeConverter
    {
        public DateTimeJsonConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddTHH:mm";
        }
    }

   
}
