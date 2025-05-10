using DataBase;
namespace Services;

// Интерфейс IAuthServices
public interface IAuthServices
{
    Task<IBaseResponse<Tokens>> TryRegister(User user, string secretKey);
    Task<IBaseResponse<Tokens>> TryLogin(LoginUser form, string secretKey);
}
