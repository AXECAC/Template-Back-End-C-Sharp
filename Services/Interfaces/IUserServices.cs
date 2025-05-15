using DataBase;
namespace Services;

// Интерфейс IUserServices
public interface IUserServices
{
    int GetMyId();

    Task<IBaseResponse<IEnumerable<User>>> GetUsers();

    Task<IBaseResponse<User>> GetUser(int id);

    Task<IBaseResponse> CreateUser(User userEntity);

    Task<IBaseResponse> DeleteUser(int id);

    Task<IBaseResponse<User>> GetUserByEmail(string email);

    Task<IBaseResponse> Edit(string oldEmail, User userEntity);
}
