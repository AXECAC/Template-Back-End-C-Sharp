using DataBase;
namespace Services;

// Интерфейс ITokenServices
public interface ITokenServices
{
    public string GenereteJWTToken(User user, string secretKey);
}
