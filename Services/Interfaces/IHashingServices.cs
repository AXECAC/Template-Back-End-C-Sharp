using DataBase;
namespace Services;

// Интерфейс IHashingServices
public interface IHashingServices
{
    User Hashing(User userEntity);
    User Hashing(LoginUser userEntity);
}
