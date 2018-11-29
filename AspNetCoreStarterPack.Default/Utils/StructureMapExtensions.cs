using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    public static class StructureMapExtensions
    {
        public static List<Instance> GetAllInstanceImplementing<TInterface>(this IContainer container) where TInterface : class
        {
            return container
                     .Model
                     .AllInstances
                     .Where(instance =>
                         !instance.ReturnedType.IsAbstract &&
                         instance.ReturnedType.GetInterfaces().Any(type => type == typeof(TInterface)) &&
                         instance.ReturnedType.GetGenericArguments().All(argument => !argument.IsGenericParameter))
                         .Select(instance => instance.Instance)
                            .ToList();
        }
    }
}
