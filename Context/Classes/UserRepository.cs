using Microsoft.EntityFrameworkCore;
using DataBase;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
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
        Console.WriteLine("Начало работы");
        User? user = null;
        // пытаемся получить данные из кэша по id
        var userString = await Cache.GetStringAsync(email);
        Console.WriteLine("Кэш пройден");
        //десериализируем из строки в объект User
        if (userString != null) user = JsonSerializer.Deserialize<User>(userString);
        Console.WriteLine(user);
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
}
