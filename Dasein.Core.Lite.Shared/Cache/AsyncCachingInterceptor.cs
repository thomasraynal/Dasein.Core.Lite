using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap.DynamicInterception;

namespace Dasein.Core.Lite.Shared
{
    public class AsyncCachingInterceptor : IAsyncInterceptionBehavior, ICanLog
    {
        public async Task<IMethodInvocationResult> InterceptAsync(IAsyncMethodInvocation methodInvocation)
        {
            var cache = methodInvocation.MethodInfo.GetCustomAttributes(typeof(Cached), false).FirstOrDefault();

            if (cache != null)
            {
                var cacheObject = new MethodCacheObject(methodInvocation);

                var cacheManager = AppCore.Instance.Get<ICacheStrategy<MethodCacheObject>>();

                var cachedObject = cacheManager.MemoryCache.Get(cacheObject.Key).Result;

                if (null != cachedObject)
                {

                    if (DateTime.Now > cachedObject.Expiration)
                    {
                        cacheManager.MemoryCache.Remove(cachedObject.Key).Wait();
                    }
                    else
                    {
                        this.LogInformation($"{nameof(AsyncCachingInterceptor)} - {methodInvocation.MethodInfo.Name}");

                        return methodInvocation.CreateResult(cachedObject.Value);
                    }
                }

                var result = await methodInvocation.InvokeNextAsync();

                cacheObject.Value = result.GetReturnValueOrThrow();

                if (null == cacheObject.Value) return result;

                cacheObject.Expiration = DateTime.Now.AddMilliseconds(((Cached)cache).Duration);

                await cacheManager.MemoryCache.Put(cacheObject.Key, cacheObject);

                return result;

            }

            return await methodInvocation.InvokeNextAsync();
        }
    }
}
