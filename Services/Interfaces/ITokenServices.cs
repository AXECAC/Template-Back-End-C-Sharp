using DataBase;
namespace Services;

// Interface ITokenServices
public interface ITokenServices
{
    public string GenereteJWTToken(User user, string secretKey);
}
