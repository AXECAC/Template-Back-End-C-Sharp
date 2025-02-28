using Microsoft.EntityFrameworkCore;
using DataBase;

namespace Context;

// Class UserRepository
public class UserRepository : BaseRepository<User> ,IUserRepository
{
    private readonly TemplateDbContext Db;

    public UserRepository(TemplateDbContext db) : base(db)
    {
        Db = db;
    }
    
    // GeUser user by email
    public async Task<User>? GetByEmail(string email)
    {
        return await Db.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    // GeUser model from db by id
    public async Task<User>? Get(int id)
    {
        return await Db.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}
