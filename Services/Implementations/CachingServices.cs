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

    // Получаем model по (int) из кэша
    public async Task<T> GetAsync(int value)
    {
        T? model = null;
        // пытаемся получить данные из кэша по value
        string modelString = await Cache.GetStringAsync(value.ToString());
        // десериализируем из строки в объект
        if (modelString != null)
        {
            model = JsonSerializer.Deserialize<T>(modelString);
        }
        return model;
    }

    // Получаем model по (double) из кэша
    public async Task<T> GetAsync(double value)
    {
        T? model = null;
        // пытаемся получить данные из кэша по value
        string modelString = await Cache.GetStringAsync(value.ToString());
        // десериализируем из строки в объект
        if (modelString != null)
        {
            model = JsonSerializer.Deserialize<T>(modelString);
        }
        return model;

    }

    // Получаем model по (string) из кэша
    public async Task<T> GetAsync(string value)
    {
        T? model = null;
        // пытаемся получить данные из кэша по value
        string modelString = await Cache.GetStringAsync(value);
        // десериализируем из строки в объект
        if (modelString != null)
        {
            model = JsonSerializer.Deserialize<T>(modelString);
        }
        return model;

    }

    // Добавляем value с key в кэша
    public async void SetAsync(T value, string key)
    {
        // сериализуем данные в строку в формате json
        string modelString = JsonSerializer.Serialize<T>(value);
        // сохраняем строковое представление объекта в формате json в кэш на 2 минуты
        await Cache.SetStringAsync(key, modelString, new DistributedCacheEntryOptions
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
