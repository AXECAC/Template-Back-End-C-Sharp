using DataBase;
namespace Services;

// Interface IHashingServices
public interface IHashingServices
{
    User Hashing(User userModel);
    User Hashing(LoginUser userModel);
}
