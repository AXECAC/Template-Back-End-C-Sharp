using Context;
using DataBase;
using Services.Caching;
namespace Services;

// Class UserServices
public class UserServices : IUserServices
{
    private readonly IUserRepository _UserRepository;
    private readonly IHashingServices _HashingServices;
    private readonly ICachingServices<User> _CachingServices;


    public UserServices(IUserRepository userRepository, IHashingServices hashingServices, ICachingServices<User> cachingServices)
    {
        _UserRepository = userRepository;
        _HashingServices = hashingServices;
        _CachingServices = cachingServices;
    }

    public async Task<IBaseResponse<IEnumerable<User>>> GetUsers()
    {
        BaseResponse<IEnumerable<User>> baseResponse;
        var users = await _UserRepository.Select();
        // in future try !users.Any()
        // Ok (204) but 0 elements
        if (users.Count == 0)
        {
            baseResponse = BaseResponse<IEnumerable<User>>.NoContent("Find 0 elements");
            return baseResponse;
        }
        // Ok (200)
        baseResponse = BaseResponse<IEnumerable<User>>.Ok(users);
        return baseResponse;
    }

    public async Task<IBaseResponse<User>> GetUser(int id)
    {
        BaseResponse<User> baseResponse;
        // Ищем User в кэше
        User? user = await _CachingServices.GetAsync(id);

        if (user == null)
        {
            // Ищем User в БД
            user = await _UserRepository.FirstOrDefaultAsync(x => x.Id == id);
            _CachingServices.SetAsync(user, user.Id.ToString());
        }
        // NotFound (404)
        if (user == null)
        {
            baseResponse = BaseResponse<User>.NotFound("User not found");
            return baseResponse;
        }
        // Found - Ok (200)
        baseResponse = BaseResponse<User>.Ok(user, "User found");
        baseResponse.Data = user;
        return baseResponse;
    }

    public async Task<IBaseResponse<User>> CreateUser(User userModel)
    {
        // Hashing Password
        userModel = _HashingServices.Hashing(userModel);
        await _UserRepository.Create(userModel);
        var baseResponse = BaseResponse<User>.Created("User created");
        return baseResponse;
    }

    public async Task<IBaseResponse<bool>> DeleteUser(int id)
    {
        BaseResponse<bool> baseResponse;

        // Ищем User в кэше
        User? user = await _CachingServices.GetAsync(id);
        // User есть в кэше
        if (user != null)
        {
            // Удаляем User из кеша
            _CachingServices.RemoveAsync(user.Id.ToString());
        }

        user = await _UserRepository.FirstOrDefaultAsync(x => x.Id == id);
        // User not found (404)
        if (user == null)
        {
            baseResponse = BaseResponse<bool>.NotFound("User not found");
            return baseResponse;
        }
        // User found (204)
        await _UserRepository.Delete(user);
        baseResponse = BaseResponse<bool>.NoContent();
        return baseResponse;
    }

    public async Task<IBaseResponse<User>> GetUserByEmail(string email)
    {
        BaseResponse<User> baseResponse;
        var user = await _UserRepository.FirstOrDefaultAsync(x => x.Email == email);
        // User not found (404)
        if (user == null)
        {
            baseResponse = BaseResponse<User>.NotFound("User not found");
            return baseResponse;
        }

        // User found (200)
        baseResponse = BaseResponse<User>.Ok(user);
        return baseResponse;
    }

    public async Task<IBaseResponse<User>> Edit(string oldEmail, User userModel)
    {
        // Hashing Password
        userModel = _HashingServices.Hashing(userModel);

        BaseResponse<User> baseResponse;
        var user = await _UserRepository.FirstOrDefaultAsync(x => x.Email == oldEmail);

        // User not found (404)
        if (user == null)
        {
            baseResponse = BaseResponse<User>.NotFound("User not found");
            return baseResponse;
        }

        // User found
        user.Email = userModel.Email;
        user.Password = userModel.Password;
        user.FirstName = userModel.FirstName;
        user.SecondName = userModel.SecondName;

        // User edit (201)
        await _UserRepository.Update(user);

        baseResponse = BaseResponse<User>.Created();
        return baseResponse;
    }
}
