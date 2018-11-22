using StructureMap;
using StructureMap.DynamicInterception;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Cache
{
    public static class RegistryCacheExtension
    {
        public static void RegisterCacheProxy<TPlugin, TConcrete>(this Registry registry)
            where TPlugin : class
            where TConcrete : class, TPlugin
        {
            registry.For<TPlugin>().Use<TConcrete>()
                .Singleton()
                .InterceptWith(new DynamicProxyInterceptor<TPlugin>(new IInterceptionBehavior[]
                {
                           new AsyncCachingInterceptor()
                }));
        }
    }
}
