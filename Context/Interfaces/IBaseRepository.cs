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
}
