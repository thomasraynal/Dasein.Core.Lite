using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Infrastructure
{
    public class DateTimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader.Value is String)
            {
                return String.IsNullOrEmpty((String)reader.Value) ? DateTime.MinValue : DateTime.Parse((String)reader.Value);
            }

            return reader.Value == null ? DateTime.MinValue : (DateTime)reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
            writer.WriteValue(date.ToString("yyyy-MM-ddTHH:mm"));
        }
    }
}
