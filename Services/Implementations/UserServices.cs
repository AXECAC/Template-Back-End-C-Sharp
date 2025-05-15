using Microsoft.AspNetCore.Http;
using Context;
using DataBase;
using System.Security.Claims;
using Services.Caching;
namespace Services;

// Класс UserServices
public class UserServices : IUserServices
{
    private readonly IHttpContextAccessor _HttpContextAccessor;
    private readonly IUserRepository _UserRepository;
    private readonly IHashingServices _HashingServices;
    private readonly ICachingServices<User> _CachingServices;


    public UserServices(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,
            IHashingServices hashingServices, ICachingServices<User> cachingServices)
    {
        _HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _UserRepository = userRepository;
        _HashingServices = hashingServices;
        _CachingServices = cachingServices;
    }
    public int GetMyId()
    {
        return Convert.ToInt32(_HttpContextAccessor.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
    }

    public async Task<IBaseResponse<IEnumerable<User>>> GetUsers()
    {
        BaseResponse<IEnumerable<User>> baseResponse;
        // Ищем всех User в БД
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
        }

        // NotFound (404)
        if (user == null)
        {
            baseResponse = BaseResponse<User>.NotFound("User not found");
            return baseResponse;
        }
        // Found - Ok (200)
        // Добавляем User в кэш
        _CachingServices.SetAsync(user, user.Id.ToString());
        baseResponse = BaseResponse<User>.Ok(user, "User found");
        baseResponse.Data = user;
        return baseResponse;
    }

    public async Task<IBaseResponse> CreateUser(User userEntity)
    {
        // Хэширование Password
        userEntity = _HashingServices.Hashing(userEntity);
        // Создаем User
        userEntity.Id = 0;
        await _UserRepository.Create(userEntity);
        var baseResponse = BaseResponse.Created("User created");
        return baseResponse;
    }

    public async Task<IBaseResponse> DeleteUser(int id)
    {
        BaseResponse baseResponse;

        // Ищем User в кэше по Id

        User? user = await _CachingServices.GetAsync(id);
        // User есть в кэше
        if (user != null)
        {
            // Удаляем User из кеша по Id

            _CachingServices.RemoveAsync(user.Id.ToString());
        }
        // Ищем User в БД
        user = await _UserRepository.FirstOrDefaultAsync(x => x.Id == id);

        // Ищем User в кэше по Email
        var userInCache = await _CachingServices.GetAsync(user.Email);

        // User есть в кэше
        if (userInCache != null)
        {
            // Удаляем User из кеша по Email

            _CachingServices.RemoveAsync(userInCache.Email);
        }

        // User не найден (404)
        if (user == null)
        {
            baseResponse = BaseResponse.NotFound("User not found");
            return baseResponse;
        }
        // User Найден (204)
        await _UserRepository.Delete(user);
        baseResponse = BaseResponse.NoContent();
        return baseResponse;
    }

    public async Task<IBaseResponse<User>> GetUserByEmail(string email)
    {
        BaseResponse<User> baseResponse;
        // Ищем User в кэше
        User? user = await _CachingServices.GetAsync(email);

        if (user == null)
        {
            // Ищем User в БД
            user = await _UserRepository.FirstOrDefaultAsync(x => x.Email == email);
        }
        // User not found (404)
        if (user == null)
        {
            baseResponse = BaseResponse<User>.NotFound("User not found");
            return baseResponse;
        }

        // User found (200)
        // Добавляем User в кэш
        _CachingServices.SetAsync(user, user.Email);
        baseResponse = BaseResponse<User>.Ok(user);
        return baseResponse;
    }

    public async Task<IBaseResponse> Edit(string oldEmail, User userEntity)
    {
        // Хэширование Password
        userEntity = _HashingServices.Hashing(userEntity);

        BaseResponse baseResponse;
        // Ищем User в кэше по Id
        User? user = await _CachingServices.GetAsync(userEntity.Id);

        if (user != null)
        {
            // Удаляем старого User по Id
            _CachingServices.RemoveAsync(user.Id.ToString());
        }
        // Ищем User в кэше по oldEmail
        user = await _CachingServices.GetAsync(oldEmail);

        if (user != null)
        {
            // Удаляем старого User по oldEmail
            _CachingServices.RemoveAsync(oldEmail);
        }
        // Ищем User в БД
        user = await _UserRepository.FirstOrDefaultAsync(x => x.Email == oldEmail);

        // User not found (404)
        if (user == null)
        {
            baseResponse = BaseResponse.NotFound("User not found");
            return baseResponse;
        }

        // User found
        user.Email = userEntity.Email;
        user.Password = userEntity.Password;
        user.FirstName = userEntity.FirstName;
        user.SecondName = userEntity.SecondName;

        // User edit (201)
        await _UserRepository.Update(user);

        // Добавляем измененного User
        _CachingServices.SetAsync(user, user.Id.ToString());

        baseResponse = BaseResponse.Created();
        return baseResponse;
    }
}
