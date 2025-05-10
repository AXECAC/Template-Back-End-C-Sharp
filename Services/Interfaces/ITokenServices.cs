using DataBase;
namespace Services;

// Интерфейс ITokenServices
public interface ITokenServices
{
    public string GenerateJWTTocken(User user, string secretKey);
}
