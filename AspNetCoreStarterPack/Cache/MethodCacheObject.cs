using AspNetCoreStarterPack.Infrastructure;
using Microsoft.AspNetCore.Http;
using StructureMap.DynamicInterception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCoreStarterPack.Cache
{
    public class MethodCacheObject
    {
        public MethodCacheObject(IMethodInvocation methodInvocation)
        {

            var cache = (Cached)methodInvocation.MethodInfo.GetCustomAttributes(typeof(Cached), false).First();

            MethodName = $"{methodInvocation.MethodInfo.ReflectedType}.{methodInvocation.MethodInfo.Name}";

            ResultType = methodInvocation.MethodInfo.ReturnType;

            var args = methodInvocation.Arguments.Count > 0 ? methodInvocation.Arguments.Select(arg => arg.Value.GetHashCode()).Aggregate((val1, val2) => val1 * 397 ^ val2) : 0;

            Key = ((args ^ MethodName.GetHashCode()) * 397).ToString();

            var invalidationKeys = methodInvocation.Arguments.Where(arg => arg.Value is ICachedRessource);

            if (invalidationKeys.Any())
            {
                var keys = invalidationKeys
                    .Select(key => key.Value)
                    .Cast<ICachedRessource>()
                    .Select(key => key.GetCacheInvalidationTags())
                    .Aggregate((tags1, tags2) =>
                    {
                        return tags1.Concat(tags2);

                    }).Distinct();

                Key = $"{keys.Aggregate((str1, str2) => str1 + "_" + str2)}-{Key}";
            }

        }

        public String Key { get; set; }
        public String MethodName { get; set; }
        public object Value { get; set; }
        public Type ResultType { get; set; }
        public DateTime Expiration { get; set; }
    }
}
