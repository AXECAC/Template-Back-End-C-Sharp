using System.Security.Cryptography;
using DataBase;
namespace Services;

// Класс HashingServices
public class HashingServices : IHashingServices
{
    private static string HashFunc(string input)
    {
        var md5 = MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hash);
    }

    public User Hashing(User user)
    {
        user.Password = HashFunc(user.Password);
        return user;
    }

    public User Hashing(LoginUser loginUser)
    {
        User user = new User();
        user.Email = loginUser.Email;
        user.Password = HashFunc(loginUser.Password);
        return user;
    }
}
