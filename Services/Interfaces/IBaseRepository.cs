namespace Services;

// Interface IBaseRepository
public interface IBaseRepository<T>
{
    // Create model in db
    bool Create(T model);

    // Get model from db by id
    T Get(int id);

    // Get models from db
    List<T> Select();

    // Delete models from db
    bool Delete(T model);

    // Update model in db
    T Update(T model);
}
