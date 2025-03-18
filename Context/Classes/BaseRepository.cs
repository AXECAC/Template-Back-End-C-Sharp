using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace Context;

// Класс BaseRepository
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly TemplateDbContext Db;
    public BaseRepository(TemplateDbContext db)
    {
        Db = db;
    }

    // Создать сущность в Db 
    public async Task<bool> Create(T entity)
    {
        await Db.Set<T>().AddAsync(entity);
        await Db.SaveChangesAsync();
        return true;
    }

    // Взять сущности из Db
    public async Task<List<T>> Select()
    {
        return await Db.Set<T>().ToListAsync();
    }

    // Удалить сущности из Db
    public async Task<bool> Delete(T entity)
    {
        Db.Set<T>().Remove(entity);
        await Db.SaveChangesAsync();

        return true;
    }

    // Обновить сущность в Db
    public async Task<T> Update(T entity)
    {
        Db.Set<T>().Update(entity);
        await Db.SaveChangesAsync();

        return entity;
    }

    // Найти сущность в Db с помощью выражения
    public async Task<T>? FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
    {
        return await Db.Set<T>().FirstOrDefaultAsync(expression);
    }

    // Получить IQueryable
    public IQueryable<T> GetQueryable()
    {
        return Db.Set<T>();
    }

    // Получить IQueryable где используется выражение
    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return Db.Set<T>().Where(expression);
    }

}
