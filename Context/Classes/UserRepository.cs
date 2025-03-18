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
}
