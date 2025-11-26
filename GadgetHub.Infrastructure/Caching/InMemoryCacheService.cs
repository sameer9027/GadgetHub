
using GadgetHub.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GadgetHub.Application.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<InMemoryCacheService> _logger;

        public InMemoryCacheService(IMemoryCache memoryCache, ILogger<InMemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public Task<T> GetAsync<T>(string cacheKey)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out T cachedData))
                {
                    _logger.LogDebug("Cache hit for key: {CacheKey}", cacheKey);
                    return Task.FromResult(cachedData);
                }

                _logger.LogDebug("Cache miss for key: {CacheKey}", cacheKey);
                return Task.FromResult(default(T));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cache key: {CacheKey}", cacheKey);
                // Returns default to indicate cache failure, so service can fall back to DB
                return Task.FromResult(default(T));
            }
        }

        public Task SetAsync<T>(string cacheKey, T data, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null)
        {
            try
            {
                if (data == null) return Task.CompletedTask;

                var cacheOptions = new MemoryCacheEntryOptions();

                if (slidingExpiration.HasValue)
                    cacheOptions.SetSlidingExpiration(slidingExpiration.Value);

                if (absoluteExpiration.HasValue)
                    cacheOptions.SetAbsoluteExpiration(absoluteExpiration.Value);
                else
                    cacheOptions.SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _memoryCache.Set(cacheKey, data, cacheOptions);
                _logger.LogDebug("Cache set for key: {CacheKey}", cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache key: {CacheKey}", cacheKey);
                //caching should not break the application
            }

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string cacheKey)
        {
            try
            {
                _memoryCache.Remove(cacheKey);
                _logger.LogDebug("Cache removed for key: {CacheKey}", cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache key: {CacheKey}", cacheKey);
               
            }

            return Task.CompletedTask;
        }
    }
}