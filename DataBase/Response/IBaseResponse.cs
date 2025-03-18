namespace DataBase;

// Интерфейс IBaseResponse
public interface IBaseResponse<T>
{
    string Description { get; }
    StatusCodes StatusCode { get; }
    T Data { get; }
}
