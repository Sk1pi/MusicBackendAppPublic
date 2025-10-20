using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace MusicBackendApp.Application.Common.Extensions;

public static class DistributedCacheExtensions
{
    //public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    public static async Task SetRecordAsync<T>(
        this IDistributedCache cache, 
        string recordId, 
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? slidingExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();
        
        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(5); 
        options.SlidingExpiration = slidingExpireTime ?? TimeSpan.FromSeconds(10);
        
        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<T?> GetRecordAsync<T>(
        this IDistributedCache cache,
        string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);
        
        //Possible null reference return
        if(jsonData is null)
            return default(T);
        
        return JsonSerializer.Deserialize<T>(jsonData);
    }
}