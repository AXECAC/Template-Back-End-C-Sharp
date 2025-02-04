using Microsoft.EntityFrameworkCore;
namespace Context;

// Class Context
public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {

    }
}
