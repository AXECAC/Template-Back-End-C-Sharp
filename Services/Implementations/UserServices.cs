using Context;
using DataBase;
namespace Services;

// Class UserServices
public class UserServices : IUserServices
{
    private readonly IUserRepository _UserRepository;
    private readonly IHashingServices _HashingServices;

    public UserServices(IUserRepository userRepository, IHashingServices hashingServices)
    {
        _UserRepository = userRepository;
        _HashingServices = hashingServices;
    }

    public async Task<IBaseResponse<IEnumerable<User>>> GetUsers()
    {
        BaseResponse<IEnumerable<User>> baseResponse;
        try
        {
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
        catch (Exception ex)
        {
            // Server error (500)
			return BaseResponse<IEnumerable<User>>.InternalServerError($"{GetUsers} : {ex.Message}");
        }
    }

    public async Task<IBaseResponse<User>> GetUser(int id)
    {
        BaseResponse<User> baseResponse;
        try
        {
            var user = await _UserRepository.Get(id);
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
        catch (Exception ex)
        {
            // Server error (500)
			return BaseResponse<User>.InternalServerError($"{GetUser} : {ex.Message}");
        }
    }

    public async Task<IBaseResponse<User>> CreateUser(User userModel)
    {
        // Hashing Password
        userModel = _HashingServices.Hashing(userModel);
        try
        {
            await _UserRepository.Create(userModel);
			var baseResponse = BaseResponse<User>.Created("User created");
            return baseResponse;
        }
        catch (Exception ex)
        {
            // Server error (500)
			return BaseResponse<User>.InternalServerError($"{CreateUser} : {ex.Message}");
        }
    }

    public async Task<IBaseResponse<bool>> DeleteUser(int id)
    {
        BaseResponse<bool> baseResponse;
        try
        {
            var user = await _UserRepository.Get(id);
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
        catch (Exception ex)
        {
            // Server error (500)
			return BaseResponse<bool>.InternalServerError($"{DeleteUser} : {ex.Message}");
        }
    }

    public async Task<IBaseResponse<User>> GetUserByEmail(string email)
    {
        BaseResponse<User> baseResponse;
        try
        {
            var user = await _UserRepository.GetByEmail(email);
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
        catch (Exception ex)
        {
            // Server error (500)
			return BaseResponse<User>.InternalServerError($"{GetUserByEmail} : {ex.Message}");
        }
    }

    public async Task<IBaseResponse<User>> Edit(string oldEmail, User userModel)
    {
        // Hashing Password
        userModel = _HashingServices.Hashing(userModel);

        BaseResponse<User> baseResponse;
        try
        {
            var user = await _UserRepository.GetByEmail(oldEmail);

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
        catch (Exception ex)
        {
            // Server error (500)
			return BaseResponse<User>.InternalServerError($"{Edit} : {ex.Message}");
        }
    }
}
