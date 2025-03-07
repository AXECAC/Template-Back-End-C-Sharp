namespace DataBase;

// Class BaseResponse
public class BaseResponse<T> : IBaseResponse<T>
{
	public string Description { get; set; }

	public StatusCodes StatusCode { get; set; }

	public T Data { get; set; }

	// Ok response generate
	public static BaseResponse<T> Ok(T data, string description = "")
	{
		return new BaseResponse<T>()
		{
			Data = data,
			StatusCode = StatusCodes.Ok,
			Description = description,
		};
	}
}
