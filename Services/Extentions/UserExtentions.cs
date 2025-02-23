using DataBase;

namespace Extentions;

// Class UserExtentions
public static class UserExtentions
{
    public static bool IsValid(this User user)
    {
        if (user.Id < 0 || user.Email.IsValidEmail() || user.Password == "" ||
                user.FirstName == "" || user.SecondName == "")
        {
            return false;
        }
        return true;
    }

    public static bool IsValidEmail(this string email)
    {
        if (email == "" || !email.Contains('@'))
        {
            return false;
        }
        return true;
    }
}
