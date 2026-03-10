using System.Linq.Expressions;

namespace WearCast.Api.Common.Repository;

public class Repository<T> : IRepository<T> where T : BaseModel, new()
{
    private ApplicationDbContext _context;
    private DbSet<T> _dbSet;
    
    public Repository(ApplicationDbContext Context)
    {
        _context = Context;
        _dbSet = _context.Set<T>();
    }
    public async Task<T> CreateAsync(T dbRecord)
    {
        _dbSet.Add(dbRecord);
        await _context.SaveChangesAsync();
        return dbRecord;
    }
    
    public async Task HardDeleteAsync(T dbRecord)
    {
        _dbSet.Remove(dbRecord);
        await  _context.SaveChangesAsync();
    }
    
    public async Task<List<T>> GetAllAsync(bool withDeleted = false)
    {
        if (withDeleted)
        {
            return await _dbSet.ToListAsync();
        }
        return await _dbSet.Where(e => !e.IsDeleted).ToListAsync();
    }

    public IQueryable<T> Get(bool withDeleted = false)
    {
        if (withDeleted)
            return _dbSet.AsQueryable();
        return _dbSet.Where(e => !e.IsDeleted).AsQueryable();
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
    {
        if (!useNoTracking)
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        else
            return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
    }

    public async Task<T> UpdateAsync(T dbRecord)
    {
        _context.Update(dbRecord);
        await _context.SaveChangesAsync();
        return dbRecord;
    }
    
    public async Task SoftDeleteAsync(int entityId)
    {
        T entity = await GetAsync(e => e.Id == entityId);
        entity.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}