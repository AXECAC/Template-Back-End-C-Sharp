using DataBase;
namespace Services;

// Интерфейс ITokenServices
public interface ITokenServices
{
    public Tokens GenerateJWTToken(User user, string secretKey);
}
