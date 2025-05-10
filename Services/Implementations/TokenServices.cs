using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using DataBase;
namespace Services;

// Класс TokenServices
public class TokenServices : ITokenServices
{
    public Tokens GenerateJWTToken(User user, string secretKey)
    {
        Tokens jwtToken = new Tokens();
        string accessToken = GenerateAccessToken(user, secretKey);
        string refreshToken = GenerateRefreshToken(user, secretKey);
        jwtToken.AccessToken = accessToken;
        jwtToken.RefreshToken = refreshToken;
        return jwtToken;
    }

    private string GenerateAccessToken(User user, string secretKey)
    {
        var token = GenerateToken(user, secretKey);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(User user, string secretKey)
    {
        var token = GenerateToken(user, secretKey + DateTime.Now.ToString());
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken GenerateToken(User user, string secretKey)
    {
        var claims = GenerateClaims(user);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds);

        return token;
    }

    private Claim[] GenerateClaims(User user)
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

}
