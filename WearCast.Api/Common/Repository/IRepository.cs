using System.Linq.Expressions;

namespace WearCast.Api.Common.Repository;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool useTracing = true);
    Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);
    Task<T> CreateAsync(T dbRecord);
    Task<T> UpdateAsync(T dbRecord);
    Task<bool> DeleteAsync(T dbRecord);
}