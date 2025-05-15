using DataBase;
namespace Services;

// Интерфейс ITokenServices
public interface ITokenServices
{
    Task<Tokens> GenerateJWTToken(User user, string secretKey);

    Task<IBaseResponse<Tokens>> RefreshToken(string oldRefreshToken, string secretKey);

    Task<IBaseResponse> DeleteRefreshToken(int userId)
;
}
