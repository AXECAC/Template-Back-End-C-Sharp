using DataBase;
namespace Services;

// Interface IAuthServices
public interface IAuthServices
{
    Task<IBaseResponse<string>> TryRegister(User user);
    Task<IBaseResponse<string>> TryLogin(LoginUser form);
}
