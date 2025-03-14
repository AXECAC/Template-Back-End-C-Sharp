using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
namespace Context;

// Class BaseRepository
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly TemplateDbContext Db;
    private readonly IDistributedCache Cache;
    public BaseRepository(TemplateDbContext db, IDistributedCache cache)
    {
        Db = db;
        Cache = cache;
    }

    // Create model in db
    public async Task<bool> Create(T model)
    {
        await Db.Set<T>().AddAsync(model);
        await Db.SaveChangesAsync();
        return true;
    }

    // GeUser models from db
    public async Task<List<T>> Select()
    {
        return await Db.Set<T>().ToListAsync();
    }

    // Delete models from db
    public async Task<bool> Delete(T model)
    {
        Db.Set<T>().Remove(model);
        await Db.SaveChangesAsync();

        return true;
    }

    // Update model in db
    public async Task<T> Update(T model)
    {
        Db.Set<T>().Update(model);
        await Db.SaveChangesAsync();

        return model;
    }

    // Find model in db with expression
    public async Task<T>? FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
    {
        return await Db.Set<T>().FirstOrDefaultAsync(expression);
    }

}
