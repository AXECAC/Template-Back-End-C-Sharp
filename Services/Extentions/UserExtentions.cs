using DataBase;

namespace Extentions;

// Class UserExtentions
public static class UserExtentions
{
    public static bool IsValid(this User user)
    {
        if (user.Id < 0 || user.Email == "" || !user.Email.Contains('@') || user.Password == "" ||
                user.FirstName == "" || user.SecondName == "")
        {
            return false;
        }
        return true;
    }
}
