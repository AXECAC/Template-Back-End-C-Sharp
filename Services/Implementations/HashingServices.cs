using System.Security.Cryptography;
using DataBase;
namespace Services;

// Class HashingServices
public class HashingServices : IHashingServices
{
    private static string HashFunc(string input)
    {
        var md5 = MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hash);
    }
}
