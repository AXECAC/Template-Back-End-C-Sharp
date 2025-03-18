namespace DataBase;

// Class BaseResponse
public class BaseResponse<T> : IBaseResponse<T>
{
    public string Description { get; set; }

    public StatusCodes StatusCode { get; set; }

    public T Data { get; set; }

    // Ok response генерация (200)
    public static BaseResponse<T> Ok(T data, string description = "")
    {
        return new BaseResponse<T>()
        {
            Data = data,
            StatusCode = StatusCodes.Ok,
            Description = description,
        };
    }

    // Пустой Ok response генерация (200)
    public static BaseResponse<T> Ok(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.Ok,
            Description = description,
        };
    }

    // Created response генерация (201)
    public static BaseResponse<T> Created(T data, string description = "")
    {
        return new BaseResponse<T>()
        {
            Data = data,
            StatusCode = StatusCodes.Created,
            Description = description,
        };
    }

    // Пустой Created response генерация (201)
    public static BaseResponse<T> Created(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.Created,
            Description = description,
        };
    }

    // NoContent response генерация (204)
    public static BaseResponse<T> NoContent(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.NoContent,
            Description = description,
        };
    }

    // Unauthorized response генерация (401)
    public static BaseResponse<T> Unauthorized(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.Unauthorized,
            Description = description,
        };
    }

    // NotFound response генерация (404)
    public static BaseResponse<T> NotFound(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.NotFound,
            Description = description,
        };
    }

    // Conflict response генерация (409)
    public static BaseResponse<T> Conflict(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.Conflict,
            Description = description,
        };
    }

    // InternalServerError response генерация (500)
    public static BaseResponse<T> InternalServerError(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.InternalServerError,
            Description = description,
        };
    }
}
