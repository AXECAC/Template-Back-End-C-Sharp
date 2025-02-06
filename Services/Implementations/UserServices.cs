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
    }

    public async Task<IBaseResponse<User>> GetUser(int id)
    {

    }

    public async Task<IBaseResponse<User>> CreateUser(User userModel)
    {

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
