using Microsoft.EntityFrameworkCore;
using DataBase;

namespace Context;

// Class UserRepository
public class UserRepository : IUserRepository
{
    private readonly TemplateDbContext Db;

    public UserRepository(TemplateDbContext db)
    {
        Db = db;
    }
    // GeUser user by email
    public async Task<User>? GetEmail(string email)
    {
        return await Db.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    // Create model in db
    public async Task<bool> Create(User model)
    {
        await Db.Users.AddAsync(model);
        await Db.SaveChangesAsync();
        return true;
    }

    // GeUser model from db by id
    public async Task<User>? Get(int id)
    {
        return await Db.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    // GeUser models from db
    public async Task<List<User>> Select()
    {
        return await Db.Users.ToListAsync();
    }

    // Delete models from db
    public async Task<bool> Delete(User model)
    {
        Db.Users.Remove(model);
        await Db.SaveChangesAsync();

        return true;
    }

    // Update model in db
    public async Task<User> Update(User model)
    {
        Db.Users.Update(model);
        await Db.SaveChangesAsync();

        return model;
    }
}
