using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    public class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
    {
        public CamelCaseExceptDictionaryKeysResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = false,
                OverrideSpecifiedNames = true
            };
        }
    }
}
