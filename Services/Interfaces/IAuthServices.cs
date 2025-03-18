using DataBase;
namespace Services;

// Интерфейс IAuthServices
public interface IAuthServices
{
    Task<IBaseResponse<string>> TryRegister(User user, string secretKey);
    Task<IBaseResponse<string>> TryLogin(LoginUser form, string secretKey);
}
