using DataBase;

namespace Context;

// Interface IUserRepository
public interface IUserRepository : IBaseRepository<User>
{
    // Get model from db by id
    Task<User>? Get(int id);

    // Get user by email
    Task<User>? GetByEmail(string email);
}
