namespace DataBase;

// Class BaseResponse
public class BaseResponse<T> : IBaseResponse<T>
{
    public string Description { get; set; }

    public StatusCodes StatusCode { get; set; }

    public T Data { get; set; }

    // Генерация Ok response  (200)
    public static BaseResponse<T> Ok(T data, string description = "")
    {
        return new BaseResponse<T>()
        {
            Data = data,
            StatusCode = StatusCodes.Ok,
            Description = description,
        };
    }

    // Генерация пустого Ok response (200)
    public static BaseResponse<T> Ok(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.Ok,
            Description = description,
        };
    }

    // Генерация Created response (201)
    public static BaseResponse<T> Created(T data, string description = "")
    {
        return new BaseResponse<T>()
        {
            Data = data,
            StatusCode = StatusCodes.Created,
            Description = description,
        };
    }

    // Генерация пустого Created response (201)
    public static BaseResponse<T> Created(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.Created,
            Description = description,
        };
    }

    // NoContent response (204)
    public static BaseResponse<T> NoContent(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.NoContent,
            Description = description,
        };
    }

    // Генерация Unauthorized response (401)
    public static BaseResponse<T> Unauthorized(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.Unauthorized,
            Description = description,
        };
    }

    // Генерация NotFound response (404)
    public static BaseResponse<T> NotFound(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.NotFound,
            Description = description,
        };
    }

    // Генерация Conflict response (409)
    public static BaseResponse<T> Conflict(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.Conflict,
            Description = description,
        };
    }

    // Генерация InternalServerError response (500)
    public static BaseResponse<T> InternalServerError(string description = "")
    {
        return new BaseResponse<T>()
        {
            StatusCode = StatusCodes.InternalServerError,
            Description = description,
        };
    }
}
