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

    public async Task<IBaseResponse<Tokens>> TryRegister(User user, string secretKey)
    {
        // Хэширование Password
        user = _HashingServices.Hashing(user);

        BaseResponse<Tokens> baseResponse;
        //Найти User по email
        var userDb = await _UserRepository.FirstOrDefaultAsync(x => x.Email == user.Email);

        // Новый User
        if (userDb == null)
        {
            // Создать новый User
            user.Id = 0;
            await _UserRepository.Create(user);
            // Created (201)
            baseResponse = BaseResponse<Tokens>.Created(data: _TokenServices.GenerateJWTToken(user, secretKey));
        }
        // Этот email уже существует
        else
        {
            // Conflict (409)
            baseResponse = BaseResponse<Tokens>.Conflict("This email already exists");
        }
        return baseResponse;
    }

    public async Task<IBaseResponse<Tokens>> TryLogin(LoginUser form, string secretKey)
    {
        // Хэширование Password
        User user = _HashingServices.Hashing(form);

        BaseResponse<Tokens> baseResponse;
        // Найти user по email
        var userDb = await _UserRepository.FirstOrDefaultAsync(x => x.Email == user.Email);

        // User существует
        if (userDb != null)
        {
            // Сравнить хэш пароля
            if (user.Password == userDb.Password)
            {
                // Ok (200)
                baseResponse = BaseResponse<Tokens>.Ok(data: _TokenServices.GenerateJWTToken(user, secretKey));
            }
            else
            {
                // Unauthorized (401)
                baseResponse = BaseResponse<Tokens>.Unauthorized("Bad password");
            }
        }
        // User не существует
        else
        {
            // Unauthorized (401)
            baseResponse = BaseResponse<Tokens>.Unauthorized("Email not found");
        }
        return baseResponse;
    }
}
