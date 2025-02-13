namespace DataBase;

// Class BaseResponse
public class BaseResponse<T> : IBaseResponse<T>
{
    public string Description { get; set; }

    public StatusCodes StatusCode { get; set; }

    public T Data { get; set; }
}
