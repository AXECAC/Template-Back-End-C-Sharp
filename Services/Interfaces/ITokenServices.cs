using DataBase;
namespace Services;

// Интерфейс ITokenServices
public interface ITokenServices
{
    public Task<Tokens> GenerateJWTToken(User user, string secretKey);

    public Task<BaseResponse<Tokens>> RefreshToken(string oldRefreshToken, string secretKey);
}
