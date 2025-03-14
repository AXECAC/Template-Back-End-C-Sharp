namespace Services.Caching;

// Interface ICachingServices
public interface ICachingServices<T>
{
    // GetAsync(int) from cache
    Task<T> GetAsync(int value);
    // GetAsyn(double)c from cache 
    Task<T> GetAsync(double value);
    // GetAsync(string) from cache
    Task<T> GetAsync(string value);

    // SetAsync value with key to cache
    void SetAsync(T value, string key);

    // RemoveAsync value by key from cache
    void RemoveAsync(string key);
}
