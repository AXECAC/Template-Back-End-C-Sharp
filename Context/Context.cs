using Microsoft.EntityFrameworkCore;
using DataBase;
namespace Context;

// Класс Context
public class TemplateDbContext : DbContext
{
    public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
}
