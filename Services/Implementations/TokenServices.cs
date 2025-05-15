using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using DataBase;
using Context;
namespace Services;

// Класс TokenServices
public class TokenServices : ITokenServices
{
    private readonly IRefreshTokenRepository _RefreshTokenRepository;
    private readonly int accessTokenTime = 5;
    private readonly int refreshTokenTime = 1440;

    public TokenServices(IRefreshTokenRepository refreshTokenRepository)
    {
        _RefreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Tokens> GenerateJWTToken(User user, string secretKey)
    {
        Tokens jwtToken = new Tokens();

        string accessToken = GenerateAccessToken(user, secretKey);
        string refreshToken = GenerateRefreshToken(user, secretKey);

        jwtToken.AccessToken = accessToken;
        jwtToken.RefreshToken = refreshToken;

        RefreshToken saveRefreshToken = new RefreshToken();
        saveRefreshToken.Id = user.Id;
        saveRefreshToken.Token = refreshToken;

        // Находим oldRefreshToken
        var oldRefreshDB = await _RefreshTokenRepository.FirstOrDefaultAsync(token => token.Id == saveRefreshToken.Id);
        // Удаляем oldRefreshToken, если тот есть
        if (oldRefreshDB != null)
        {
            await _RefreshTokenRepository.Delete(oldRefreshDB);

        }
        await _RefreshTokenRepository.Create(saveRefreshToken);

        return jwtToken;
    }

    public async Task<IBaseResponse<Tokens>> RefreshToken(string oldRefreshToken, string secretKey)
    {
        BaseResponse<Tokens> response;

        ValidateRefreshToken(oldRefreshToken, secretKey);

        Tokens jwtToken = new Tokens();

        var oldToken = new JwtSecurityTokenHandler().ReadJwtToken(oldRefreshToken);

        User user = new User();

        // Заполняем user на основе данных из oldRefreshToken
        user.Id = Convert.ToInt32(oldToken.Claims.First(
                    claim => claim.Type == JwtRegisteredClaimNames.Sub
                    ).Value);
        user.FirstName = Convert.ToString(oldToken.Claims.First(
                    claim => claim.Type == JwtRegisteredClaimNames.Name
                    ).Value);
        user.Email = Convert.ToString(oldToken.Claims.First(
                    claim => claim.Type == JwtRegisteredClaimNames.Email
                    ).Value);

        // Находим oldRefreshToken
        var oldRefreshDB = await _RefreshTokenRepository.FirstOrDefaultAsync(token => token.Token == oldRefreshToken);

        var oldTokenDB = new JwtSecurityTokenHandler().ReadJwtToken(oldRefreshDB.Token);

        // Оставляем старый refreshToken, или в случае просрока заменим далее
        string refreshToken = oldRefreshDB.Token;

        bool tokenExpired = false;

        var createDate = DateTimeOffset.FromUnixTimeSeconds(
                Convert.ToInt64(
                    oldTokenDB.Claims.First(
                        claim => claim.Type == JwtRegisteredClaimNames.Exp
                        ).Value
                    )
                ).DateTime;

        if (DateTime.Now >= createDate)
        {
            // Удаляем oldRefreshToken, если тот истек
            await _RefreshTokenRepository.Delete(oldRefreshDB);
            // Пересоздаем oldRefreshToken, если тот истек
            refreshToken = GenerateRefreshToken(user, secretKey);

            tokenExpired = true;
        }

        string accessToken = GenerateAccessToken(user, secretKey);


        jwtToken.AccessToken = accessToken;
        jwtToken.RefreshToken = refreshToken;

        RefreshToken saveRefreshToken = new RefreshToken();
        saveRefreshToken.Id = user.Id;
        saveRefreshToken.Token = refreshToken;

        // Сохраняем новый refreshToken в бд
        if (tokenExpired)
        {
            await _RefreshTokenRepository.Create(saveRefreshToken);
        }
        response = BaseResponse<Tokens>.Ok(jwtToken);
        return response;
    }

    public async Task<IBaseResponse> DeleteRefreshToken(int userId)
    {
        BaseResponse response;

        var refreshToken = await _RefreshTokenRepository.FirstOrDefaultAsync(token => token.Id == userId);

        if (refreshToken == null)
        {
            response = BaseResponse.NoContent();
            return response;
        }
        await _RefreshTokenRepository.Delete(refreshToken);

        response = BaseResponse.Ok();
        return response;
    }

    private string GenerateAccessToken(User user, string secretKey)
    {
        var claims = GenerateClaimsAccess(user);
        var token = GenerateToken(claims, secretKey, accessTokenTime);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(User user, string secretKey)
    {
        var claims = GenerateClaimsRefresh(user);
        var token = GenerateToken(claims, secretKey + "sault", refreshTokenTime);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken GenerateToken(Claim[] claims, string secretKey, int time)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(time),
                signingCredentials: creds);

        return token;
    }

    private Claim[] GenerateClaimsAccess(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Name, user.FirstName + " " + user.SecondName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,  user.Email),
            // new Claim(ClaimTypes.Role, user.Role),
        };
        return claims;
    }

    private Claim[] GenerateClaimsRefresh(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Name, user.FirstName + " " + user.SecondName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,  user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // new Claim(ClaimTypes.Role, user.Role),
        };
        return claims;
    }

    private void ValidateRefreshToken(string token, string secretKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey + "sault")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        // Проверка подписи JWT, если токен не верный будет исключение
        tokenHandler.ValidateToken(token, validationParameters, out _);
    }

}
