namespace DataBase;

// Class BaseResponse
public class BaseResponse<T> : IBaseResponse<T>
{
	public string Description { get; set; }

	public StatusCodes StatusCode { get; set; }

	public T Data { get; set; }

	// Ok response generate (200)
	public static BaseResponse<T> Ok(T data, string description = "")
	{
		return new BaseResponse<T>()
		{
			Data = data,
			StatusCode = StatusCodes.Ok,
			Description = description,
		};
	}
	// Created response generate (201)
	public static BaseResponse<T> Created(T data, string description = "")
	{
		return new BaseResponse<T>()
		{
			Data = data,
			StatusCode = StatusCodes.Created,
			Description = description,
		};
	}
	// Empty Created response generate (201)
	public static BaseResponse<T> Created(string description = "")
	{
		return new BaseResponse<T>()
		{
			StatusCode = StatusCodes.Created,
			Description = description,
		};
	}

	// NoContent response generate (204)
	public static BaseResponse<T> NoContent(string description = "")
	{
		return new BaseResponse<T>()
		{
			StatusCode = StatusCodes.NoContent,
			Description = description,
		};
	}
}
