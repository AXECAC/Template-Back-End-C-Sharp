using Context;
using DataBase;
namespace Services;

// Class UserServices
public class UserServices : IUserServices
{
    private readonly IUserRepository UserRepository;

    public UserServices(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

    public async Task<IBaseResponse<IEnumerable<User>>> GetUsers()
    {
        var baseResponse = new BaseResponse<IEnumerable<User>>();
        try
        {
            var users = await UserRepository.Select();
            // in future try !users.Any()
            // Ok (204) but 0 elements
            if (users.Count == 0)
            {
                baseResponse.Description = "Find 0 elements";
                baseResponse.StatusCode = 204;
                return baseResponse;
            }
            // Ok (200)
            baseResponse.Data = users;
            baseResponse.StatusCode = 200;
            return baseResponse;
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<IEnumerable<User>>()
            {
                Description = $"{GetUsers} : {ex.Message}",
                StatusCode = 500,
            };
        }
    }

    public async Task<IBaseResponse<User>> GetUser(int id)
    {
        var baseResponse = new BaseResponse<User>();
        try
        {
            var user = await UserRepository.Get(id);
            // NotFound (404)
            if (user == null)
            {
                baseResponse.Description = "User not found";
                baseResponse.StatusCode = 404;
                return baseResponse;
            }
            baseResponse.Description = "User found";
            baseResponse.Data = user;
            baseResponse.StatusCode = 200;
            return baseResponse;
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<User>()
            {
                Description = $"{GetUser} : {ex.Message}",
                StatusCode = 500,
            };
        }
    }

    public async Task<IBaseResponse<User>> CreateUser(User userModel)
    {
        var baseResponse = new BaseResponse<User>();
        try
        {
            await UserRepository.Create(userModel);
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<User>()
            {
                Description = $"{GetUser} : {ex.Message}",
                StatusCode = 500,
            };
        }
        return baseResponse;
    }

    public async Task<IBaseResponse<bool>> DeleteUser(int id)
    {

    }

    public async Task<IBaseResponse<User>> GetUserByEmail(string email)
    {

    }

    public async Task<IBaseResponse<User>> Edit(int id, User userModel)
    {

    }
    
}
