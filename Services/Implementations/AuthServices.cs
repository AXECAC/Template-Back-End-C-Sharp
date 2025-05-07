using Context;
using DataBase;
namespace Services;

// Класс AuthServices
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
        // Хэширование Password
        user = _HashingServices.Hashing(user);

        BaseResponse<string> baseResponse;
        //Найти User по email
        var userDb = await _UserRepository.FirstOrDefaultAsync(x => x.Email == user.Email);

        // Новый User
        if (userDb == null)
        {
            // Создать новый User
            user.Id = 0;
            await _UserRepository.Create(user);
            // Created (201)
            baseResponse = BaseResponse<string>.Created(data: _TokenServices.GenereteJWTToken(user, secretKey));
        }
        // Этот email уже существует
        else
        {
            // Conflict (409)
            baseResponse = BaseResponse<string>.Conflict("This email already exists");
        }
        return baseResponse;
    }

    public async Task<IBaseResponse<string>> TryLogin(LoginUser form, string secretKey)
    {
        // Хэширование Password
        User user = _HashingServices.Hashing(form);

        BaseResponse<string> baseResponse;
        // Найти user по email
        var userDb = await _UserRepository.FirstOrDefaultAsync(x => x.Email == user.Email);

        // User существует
        if (userDb != null)
        {
            // Сравнить хэш пароля
            if (user.Password == userDb.Password)
            {
                // Ok (200)
                baseResponse = BaseResponse<string>.Ok(data: _TokenServices.GenereteJWTToken(user, secretKey));
            }
            else
            {
                // Unauthorized (401)
                baseResponse = BaseResponse<string>.Unauthorized("Bad password");
            }
        }
        // User не существует
        else
        {
            // Unauthorized (401)
            baseResponse = BaseResponse<string>.Unauthorized("Email not found");
        }
        return baseResponse;
    }
}
