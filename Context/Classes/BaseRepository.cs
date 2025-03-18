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

    // Создать модель в Db 
    public async Task<bool> Create(T model)
    {
        await Db.Set<T>().AddAsync(model);
        await Db.SaveChangesAsync();
        return true;
    }

    // Взять модели из Db
    public async Task<List<T>> Select()
    {
        return await Db.Set<T>().ToListAsync();
    }

    // Удалить модели из Db
    public async Task<bool> Delete(T model)
    {
        Db.Set<T>().Remove(model);
        await Db.SaveChangesAsync();

        return true;
    }

    // Обновить модель в Db
    public async Task<T> Update(T model)
    {
        Db.Set<T>().Update(model);
        await Db.SaveChangesAsync();

        return model;
    }

    // Найти модель в Db с помощью выражения
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
