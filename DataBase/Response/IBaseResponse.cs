namespace DataBase;

// Interface IBaseResponse
public interface IBaseResponse<T>
{
    StatusCodes StatusCode { get; }
    T Data { get; }
}
