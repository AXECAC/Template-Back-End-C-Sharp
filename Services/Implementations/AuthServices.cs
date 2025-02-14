using Context;
using DataBase;
namespace Services;

// Class AuthServices
public class AuthServices : IAuthServices
{
    private readonly IUserRepository _UserRepository;
    private readonly IHashingServices _HashingServices;

    public AuthServices(IUserRepository userRepository, IHashingServices hashingServices)
    {
        _UserRepository = userRepository;
        _HashingServices = hashingServices;
    }

    public async Task<IBaseResponse<string>> TryRegister(User user)
    {
        // Hashing Password
        user = _HashingServices.Hashing(user);

        var baseResponse = new BaseResponse<string>();
        try
        {
            // Find new user email
            var userDb = await _UserRepository.GetByEmail(user.Email);

            // New user
            if (userDb == null)
            {
                // Create new user
                await _UserRepository.Create(user);
                // Created (201)
                baseResponse.StatusCode = StatusCodes.Created;
                // For JWT token in future
                baseResponse.Data = "";
            }
            // This email already exists
            else
            {
                // Conflict (409)
                baseResponse.StatusCode = StatusCodes.Conflict;
                baseResponse.Description = "This email already exists";
            }
            return baseResponse;
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<string>()
            {
                Description = $"{TryRegister} : {ex.Message}",
                StatusCode = StatusCodes.InternalServerError,
            };
        }

    }

    public async Task<IBaseResponse<string>> TryLogin(LoginUser form)
    {

        var baseResponse = new BaseResponse<string>();
        try
        {
            return baseResponse;
        }
        catch (Exception ex)
        {
            // Server error (500)
            return new BaseResponse<string>()
            {
                Description = $"{TryRegister} : {ex.Message}",
                StatusCode = StatusCodes.InternalServerError,
            };
        }
    }
}
