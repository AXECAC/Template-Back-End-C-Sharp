using Microsoft.EntityFrameworkCore;
using DataBase;

namespace Context;

// Класс UserRepository
public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly TemplateDbContext Db;

    public UserRepository(TemplateDbContext db) : base(db)
    {
        Db = db;
    }

    // Получить модель User по email
    public async Task<User>? GetByEmail(string email)
    {
        return await Db.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    // Получить модель user по id
    public async Task<User>? Get(int id)
    {
        return await Db.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}
