using DataBase;

namespace Extentions;

// Класс UserExtentions
public static class UserExtentions
{
    // Валидация User
    public static bool IsValid(this User user)
    {
        if (user.Id < 0 || !user.Email.IsValidEmail() || !user.Password.IsValidPassword() ||
                user.FirstName == "" || user.SecondName == "")
        {
            return false;
        }
        return true;
    }

    // Валидация Login User
    public static bool IsValid(this LoginUser form)
    {
        if (!form.Email.IsValidEmail() || !form.Password.IsValidPassword())
        {
            return false;
        }
        return true;
    }

    // Валидация Email
    public static bool IsValidEmail(this string email)
    {
        // Продолжить в будущем
        if (email == "" || !email.Contains('@'))
        {
            return false;
        }
        return true;
    }

    // Валидация Password
    public static bool IsValidPassword(this string password)
    {
        // Продолжить в будущем
        if (password == "")
        {
            return false;
        }
        return true;
    }
}
