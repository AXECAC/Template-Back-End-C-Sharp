using Microsoft.EntityFrameworkCore;
using DataBase;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace Context;

// Class UserRepository
public class UserRepository : IUserRepository
{
    private readonly TemplateDbContext Db;
    private readonly IDistributedCache Cache;
    public UserRepository(TemplateDbContext db, IDistributedCache cache)
    {
        Db = db;
        Cache = cache;
    }

    // GeUser user by email
    public async Task<User>? GetByEmail(string email)
    {
        Console.WriteLine("Начало работы");
        User? user = null;
        // пытаемся получить данные из кэша по id
        var userString = await Cache.GetStringAsync(email);
        //десериализируем из строки в объект User
        if (userString != null) user = JsonSerializer.Deserialize<User>(userString);

        if (user == null)
        {
            Console.WriteLine("Пусто в кэше");
            user = await Db.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user != null)
            {
                Console.WriteLine($"{user.FirstName} извлечен из Базы данных");
                // сериализуем данные в строку в формате json
                userString = JsonSerializer.Serialize(user);
                // сохраняем строковое представление объекта в формате json в кэш на 2 минуты
                await Cache.SetStringAsync(user.Email, userString, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });

            }
        }
        else
        {
            Console.WriteLine($"{user.FirstName} извлечен из кэша");
        }
        return user;
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
