using Context;
using DataBase;
namespace Services;

// Class AuthServices
public class AuthServices : IAuthServices
{
    private readonly IUserRepository _UserRepository;
    private readonly IHashingServices _HashingServices;
    private readonly ITokenServices _TokenServices;

    public AuthServices(IUserRepository userRepository, IHashingServices hashingServices,
            ITokenServices tokenServices)
    {
        _UserRepository = userRepository;
        _HashingServices = hashingServices;
        _TokenServices = tokenServices;
    }

    public async Task<IBaseResponse<string>> TryRegister(User user, string secretKey)
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
                baseResponse.Data = _TokenServices.GenereteJWTToken(user, secretKey);
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

    public async Task<IBaseResponse<string>> TryLogin(LoginUser form, string secretKey)
    {
        // Hashing Password
        User user = _HashingServices.Hashing(form);

        var baseResponse = new BaseResponse<string>();
        try
        {
            // Find user email
            var userDb = await _UserRepository.GetByEmail(user.Email);

            // User exists
            if (userDb != null)
            {
                // Compare hash password
                if (user.Password == userDb.Password)
                {
                    // Ok (200)
                    baseResponse.StatusCode = StatusCodes.Ok;
                    // JWT token generate
                    baseResponse.Data = _TokenServices.GenereteJWTToken(user, secretKey);
                }
                else
                {

                    // Unauthorized (401)
                    baseResponse.StatusCode = StatusCodes.Unauthorized;
                    baseResponse.Description = "Bad password";
                }
            }
            // User not exists
            else
            {
                // Unauthorized (401)
                baseResponse.StatusCode = StatusCodes.Unauthorized;
                baseResponse.Description = "Email not found";
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
}
