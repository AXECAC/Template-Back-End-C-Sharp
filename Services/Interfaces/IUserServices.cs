using DataBase;
namespace Services;

// Interface IUserServices
public interface IUserServices
{
    Task<IBaseResponse<IEnumerable<User>>> GetUsers();

    Task<IBaseResponse<User>> GetUser(int id);

    Task<IBaseResponse<User>> CreateUser(User userModel);

    Task<IBaseResponse<bool>> DeleteUser(int id);

    Task<IBaseResponse<User>> GetUserByEmail(string email);

    Task<IBaseResponse<User>> Edit(int id, User userModel);
}
