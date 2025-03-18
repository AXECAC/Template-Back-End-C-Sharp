using System.Linq.Expressions;
namespace Context;

// Интерфейс IBaseRepository
public interface IBaseRepository<T>
{
    // Создать сущность в Db
    Task<bool> Create(T entity);

    // Получить сущности из Db
    Task<List<T>>? Select();

    // Удалить сущность из Db
    Task<bool> Delete(T entity);

    // Обновить сущность в Db
    Task<T> Update(T entity);

    // Найти сущность в Db с помощью выражения
    Task<T>? FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

    // Получить IQueryable
    IQueryable<T> GetQueryable();

    // Получить IQueryable где используется выражение
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
}
