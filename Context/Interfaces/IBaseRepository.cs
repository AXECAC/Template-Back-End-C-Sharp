using System.Linq.Expressions;
namespace Context;

// Interface IBaseRepository
public interface IBaseRepository<T>
{
    // Create model in db
    Task<bool> Create(T model);

    // Get models from db
    Task<List<T>>? Select();

    // Delete models from db
    Task<bool> Delete(T model);

    // Update model in db
    Task<T> Update(T model);

    // Find model in db with expression
    Task<T>? FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

    // Get IQueryable
    IQueryable<T> GetQueryable();
}
