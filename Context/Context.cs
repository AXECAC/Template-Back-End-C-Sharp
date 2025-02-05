using Microsoft.EntityFrameworkCore;
namespace Context;

// Class Context
public class TemplateDbContext : DbContext
{
    public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
    {

    }
}
