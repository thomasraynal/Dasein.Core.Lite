using Dasein.Core.Lite.Shared;
using StructureMap;
using StructureMap.DynamicInterception;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite
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

        public static void RegisterCacheProxyService<TPlugin, TConcrete>(this Registry registry)
        where TPlugin : class
        where TConcrete : class, TPlugin
        {
            registry.For<TPlugin>().Use<TConcrete>()
                .Singleton()
                .InterceptWith(new DynamicProxyInterceptor<TPlugin>(new IInterceptionBehavior[]
                {
                           new AsyncCachingInterceptor()
                }));

            registry.Forward<TPlugin, IService<TPlugin>>();
        }

        public static void RegisterService<TPlugin, TConcrete>(this Registry registry)
            where TPlugin : class
            where TConcrete : class, TPlugin
        {
            registry.For<TPlugin>().Use<TConcrete>().Singleton();
            registry.Forward<TPlugin, IService<TPlugin>>();
        }
    }
}
