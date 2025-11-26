
using GadgetHub.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace GadgetHub.Application.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string cacheKey);
        Task SetAsync<T>(string cacheKey, T data, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null);
        Task RemoveAsync(string cacheKey);
    }
}
