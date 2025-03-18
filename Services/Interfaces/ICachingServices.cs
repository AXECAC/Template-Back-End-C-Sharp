namespace Services.Caching;

// Интерфейс ICachingServices
public interface ICachingServices<T>
{
    // GetAsync(int) из кэша
    Task<T> GetAsync(int value);
    // GetAsync(double) из кэша 
    Task<T> GetAsync(double value);
    // GetAsync(string) из кэша
    Task<T> GetAsync(string value);

    // SetAsync значением key в кэш
    void SetAsync(T value, string key);

    // RemoveAsync значением по key из кэша
    void RemoveAsync(string key);
}
