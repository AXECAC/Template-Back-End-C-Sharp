using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace Services.Caching;

// Class CachingServices
public class CachingServices<T> : ICachingServices<T> where T : class
{
    private readonly IDistributedCache Cache;

    public CachingServices(IDistributedCache cache)
    {
        Cache = cache;
    }

    // Получаем entity по (int) из кэша
    public async Task<T> GetAsync(int value)
    {
        T? entity = null;
        // пытаемся получить данные из кэша по value
        string entityString = await Cache.GetStringAsync(value.ToString());
        // десериализируем из строки в объект
        if (entityString != null)
        {
            entity = JsonSerializer.Deserialize<T>(entityString);
        }
        return entity;
    }

    // Получаем entity по (double) из кэша
    public async Task<T> GetAsync(double value)
    {
        T? entity = null;
        // пытаемся получить данные из кэша по value
        string entityString = await Cache.GetStringAsync(value.ToString());
        // десериализируем из строки в объект
        if (entityString != null)
        {
            entity = JsonSerializer.Deserialize<T>(entityString);
        }
        return entity;

    }

    // Получаем entity по (string) из кэша
    public async Task<T> GetAsync(string value)
    {
        T? entity = null;
        // пытаемся получить данные из кэша по value
        string entityString = await Cache.GetStringAsync(value);
        // десериализируем из строки в объект
        if (entityString != null)
        {
            entity = JsonSerializer.Deserialize<T>(entityString);
        }
        return entity;

    }

    // Добавляем value с key в кэша
    public async void SetAsync(T value, string key)
    {
        // сериализуем данные в строку в формате json
        string entityString = JsonSerializer.Serialize<T>(value);
        // сохраняем строковое представление объекта в формате json в кэш на 2 минуты
        await Cache.SetStringAsync(key, entityString, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        });

    }

    // Удаляем value по key из кэша
    public async void RemoveAsync(string key)
    {
        await Cache.RemoveAsync(key);
    }
}
