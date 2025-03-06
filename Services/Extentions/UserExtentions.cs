using DataBase;

namespace Extentions;

// Class UserExtentions
public static class UserExtentions
{
    public static bool IsValid(this User user)
    {
        if (user.Id < 0 || !user.Email.IsValidEmail() || !user.Password.IsValidPassword() ||
                user.FirstName == "" || user.SecondName == "")
        {
            return false;
        }
        return true;
    }

    public static bool IsValidEmail(this string email)
    {
        // Need continue in future
        if (email == "" || !email.Contains('@'))
        {
            return false;
        }
        return true;
    }

    public static bool IsValidPassword(this string password)
    {
        // Need continue in future
        if (password == "")
        {
            return false;
        }
        return true;
    }
}
