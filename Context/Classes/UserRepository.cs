using Microsoft.EntityFrameworkCore;
using DataBase;

namespace Context;

// Class UserRepository
public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly TemplateDbContext Db;

    public UserRepository(TemplateDbContext db) : base(db)
    {
        Db = db;
    }
}
