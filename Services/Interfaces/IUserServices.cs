using DataBase;
namespace Services;

// Интерфейс IUserServices
public interface IUserServices
{
    Task<IBaseResponse<IEnumerable<User>>> GetUsers();

    Task<IBaseResponse<User>> GetUser(int id);

    Task<IBaseResponse<bool>> CreateUser(User userModel);

    Task<IBaseResponse<bool>> DeleteUser(int id);

    Task<IBaseResponse<User>> GetUserByEmail(string email);

    Task<IBaseResponse<bool>> Edit(string oldEmail, User userModel);
}
