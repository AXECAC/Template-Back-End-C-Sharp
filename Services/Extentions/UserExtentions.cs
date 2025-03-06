using DataBase;

namespace Extentions;

// Class UserExtentions
public static class UserExtentions
{
    // User validation
    public static bool IsValid(this User user)
    {
        if (user.Id < 0 || !user.Email.IsValidEmail() || !user.Password.IsValidPassword() ||
                user.FirstName == "" || user.SecondName == "")
        {
            return false;
        }
        return true;
    }

    // Login User validation
    public static bool IsValid(this LoginUser form)
    {
        if (!form.Email.IsValidEmail() || !form.Password.IsValidPassword())
        {
            return false;
        }
        return true;
    }

    // Email validation
    public static bool IsValidEmail(this string email)
    {
        // Need continue in future
        if (email == "" || !email.Contains('@'))
        {
            return false;
        }
        return true;
    }

    // Password validation
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
