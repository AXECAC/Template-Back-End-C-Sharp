using System.Linq.Expressions;
namespace Context;

// Интерфейс IBaseRepository
public interface IBaseRepository<T>
{
    // Создать модель в бд
    Task<bool> Create(T model);

    // Получить модели из бд
    Task<List<T>>? Select();

    // Удалить модель из бд
    Task<bool> Delete(T model);

    // Обновить модель в бд
    Task<T> Update(T model);

    // Найти модель в бд с помощью выражения
    Task<T>? FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

    // Получить IQueryable
    IQueryable<T> GetQueryable();

    // Получить IQueryable где используется выражение
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
}
