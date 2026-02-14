using System.Linq.Expressions;

namespace WearCast.Api.Common.Repository;

public interface IRepository<T>
{
    public Task<List<T>> GetAllAsync();
    public Task<T> GetAsync(Expression<Func<T, bool>>filter,  bool useNoTracking = false);
    public Task<T> CreateAsync(T dbRecord);
    public Task<T> UpdateAsync(T dbRecord);
    public Task HardDeleteAsync(T dbRecord);
    public Task SoftDeleteAsync(Guid  entityId);
}