using Yarp.ReverseProxy.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Threading;

namespace QuickCode.MyecommerceDemo.Gateway.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IReverseProxyBuilder LoadFromMemory(this IReverseProxyBuilder builder)
        {
            builder.Services.AddSingleton<InMemoryConfigProvider>();
            builder.Services.AddSingleton<IHostedService>(ctx => ctx.GetRequiredService<InMemoryConfigProvider>());
            builder.Services.AddSingleton<IProxyConfigProvider>(ctx => ctx.GetRequiredService<InMemoryConfigProvider>());
            return builder;
        }
        
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> KeyLocks = new();

        public static async Task<T> GetOrAddAsync<T>(
            this IMemoryCache cache,
            string key,
            Func<object[], Task<T>> valueFactory,
            TimeSpan expiration,
            params object[] args)
        {
            if (cache.TryGetValue(key, out T cachedValue))
                return cachedValue!;

            var keyLock = KeyLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
            
            await keyLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(key, out cachedValue!))
                    return cachedValue;

                cachedValue = await valueFactory(args);
                cache.Set(key, cachedValue, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                });
            }
            finally
            {
                keyLock.Release();
                KeyLocks.TryRemove(key, out _); 
            }
            return cachedValue;
        }

        public static async Task<T> GetOrAddAsync<T>(
            this IMemoryCache cache,
            string key,
            Func<object[], Task<T>> valueFactory,
            params object[] args)
        {
            return await cache.GetOrAddAsync(key, valueFactory, TimeSpan.MaxValue, args);
        }
    }
}