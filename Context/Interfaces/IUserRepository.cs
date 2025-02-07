using DataBase;

namespace Context;

// Interface IUserRepository
public interface IUserRepository : IBaseRepository<User>
{
    // Get user by email
    Task<User>? GetByEmail(string email);
}
