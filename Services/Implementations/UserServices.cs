using Context;
using DataBase;
namespace Services;

// Class UserServices
public class UserServices : IUserServices
{
    private readonly IUserRepository _UserRepository;

    public UserServices(IUserRepository userRepository)
    {
        _UserRepository = userRepository;
    }

    public async Task<IBaseResponse<IEnumerable<User>>> GetUsers()
    {
        var baseResponse = new BaseResponse<IEnumerable<User>>();
        try
        {
            var users = await _UserRepository.Select();
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
            var user = await _UserRepository.Get(id);
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
            await _UserRepository.Create(userModel);
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<User>()
            {
                Description = $"{CreateUser} : {ex.Message}",
                StatusCode = 500,
            };
        }
        return baseResponse;
    }

    public async Task<IBaseResponse<bool>> DeleteUser(int id)
    {
        var baseResponse = new BaseResponse<bool>()
        {
            StatusCode = 204,
            Data = true
        };
        try
        {
            var user = await _UserRepository.Get(id);
            if (user == null)
            {
                baseResponse.Description = "User not found";
                baseResponse.StatusCode = 404;
                baseResponse.Data = false;

                return baseResponse;
            }

            await _UserRepository.Delete(user);

            return baseResponse;
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<bool>()
            {
                Description = $"{DeleteUser} : {ex.Message}",
                StatusCode = 500,
            };
        }
    }

    public async Task<IBaseResponse<User>> GetUserByEmail(string email)
    {
        var baseResponse = new BaseResponse<User>();
        try
        {
            var user = await _UserRepository.GetByEmail(email);
            if (user == null)
            {
                baseResponse.Description = "User not found";
                baseResponse.StatusCode = 404;
                return baseResponse;
            }

            baseResponse.Data = user;
            return baseResponse;
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<User>()
            {
                Description = $"{GetUserByEmail} : {ex.Message}",
                StatusCode = 500,
            };
        }
    }

    public async Task<IBaseResponse<User>> Edit(int id, User userModel)
    {
        var baseResponse = new BaseResponse<User>();
        try
        {
            var user = await _UserRepository.Get(id);
            if (user == null)
            {
                baseResponse.StatusCode = 404;
                baseResponse.Description = "User not found";
                return baseResponse;
            }

            user.Email = userModel.Email;
            user.Password = userModel.Password;
            user.FirstName = userModel.FirstName;
            user.SecondName = userModel.SecondName;

            await _UserRepository.Update(user);

            return baseResponse;
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<User>()
            {
                Description = $"{Edit} : {ex.Message}",
                StatusCode = 500,
            };
        }
    }
}
