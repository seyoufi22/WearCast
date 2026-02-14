using System.Linq.Expressions;

namespace WearCast.Api.Common.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public async Task<T> CreateAsync(T dbRecord)
    {
        _dbSet.Add(dbRecord);
        await _context.SaveChangesAsync();
        return dbRecord;
    }

    public async Task<bool> DeleteAsync(T dbRecord)
    {
        _dbSet.Remove(dbRecord);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool useTracing = true)
    {
        if (useTracing)
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
    }

    public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.Where(filter).FirstOrDefaultAsync();
    }

    public async Task<T> UpdateAsync(T dbRecord)
    {
        _dbSet.Update(dbRecord);
        await _context.SaveChangesAsync();
        return dbRecord;
    }
}