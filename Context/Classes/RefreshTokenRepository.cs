using DataBase;
namespace Context;

// Класс RefreshTokenRepository
public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    private readonly TemplateDbContext Db;
    public RefreshTokenRepository(TemplateDbContext db) : base(db)
    {
        Db = db;
    }
}
