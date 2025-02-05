using Microsoft.EntityFrameworkCore;
namespace Context;

// Class Context
public class TepmlateDbContext : DbContext
{
    public TepmlateDbContext(DbContextOptions<TepmlateDbContext> options) : base(options)
    {

    }
}
