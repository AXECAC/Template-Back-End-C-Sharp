using DataBase;

namespace Services;

// Interface IUserRepository
public interface IUserRepository : IBaseRepository<User>
{
    // Get user by email
    Task<User>? GetEmail(string email);
}
