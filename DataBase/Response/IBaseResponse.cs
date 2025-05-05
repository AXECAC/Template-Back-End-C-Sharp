namespace DataBase;

// Интерфейс IBaseResponse
public interface IBaseResponse<T>
{
    string Description { get; }
    StatusCodes StatusCode { get; }
    T Data { get; }
}

// Интерфейс IBaseResponse без data
public interface IBaseResponse
{
    string Description { get; }
    StatusCodes StatusCode { get; }
}
