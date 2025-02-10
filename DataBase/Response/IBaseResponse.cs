namespace DataBase;

// Interface IBaseResponse
public interface IBaseResponse<T>
{
    int StatusCode { get; }
    T Data { get; }
}
