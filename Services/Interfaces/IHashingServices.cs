using DataBase;
namespace Services;

// Интерфейс IHashingServices
public interface IHashingServices
{
    User Hashing(User userModel);
    User Hashing(LoginUser userModel);
}
