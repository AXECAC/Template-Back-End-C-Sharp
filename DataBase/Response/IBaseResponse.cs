namespace DataBase;

// Interface IBaseResponse
public interface IBaseResponse<T>
{
	string Description { get; }
	StatusCodes StatusCode { get; }
	T Data { get; }
}
