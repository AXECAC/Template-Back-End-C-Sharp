using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using DataBase;
namespace Services;

// Class TokenServices
public class TokenServices : ITokenServices
{
    public string GenereteJWTToken(User user, string secretKey)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.SecondName + user.FirstName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            // For future where add roles
            // new Claim(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
