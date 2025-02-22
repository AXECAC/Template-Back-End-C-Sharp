using DataBase;
namespace Services;

// Interface IAuthServices
public interface IAuthServices
{
    Task<IBaseResponse<string>> TryRegister(User user, string secretKey);
    Task<IBaseResponse<string>> TryLogin(LoginUser form, string secretKey);
}
