using System.Linq.Expressions;

namespace WearCast.Api.Common.Repository;

public interface IRepository<T>
{
    public Task<List<T>> GetAllAsync(bool withDeleted = false);
    public Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter, bool withDeleted = false);
    Task<T> GetAsync(
    Expression<Func<T, bool>> filter,
    bool useNoTracking = false,
    Func<IQueryable<T>, IQueryable<T>>? include = null
    );
    public Task<T> CreateAsync(T dbRecord);
    public Task<T> UpdateAsync(T dbRecord);
    public Task HardDeleteAsync(T dbRecord);
    public Task SoftDeleteAsync(int entityId);
}