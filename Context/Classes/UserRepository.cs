using Microsoft.EntityFrameworkCore;
using DataBase;
using Microsoft.Extensions.Caching.Distributed;
namespace Context;

// Class UserRepository
public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly TemplateDbContext Db;
    private readonly IDistributedCache Cache;
    public UserRepository(TemplateDbContext db, IDistributedCache cache) : base(db, cache)
    {
        Db = db;
        Cache = cache;
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
