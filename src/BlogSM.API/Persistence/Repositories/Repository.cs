using BlogSM.API.Persistence.Repositories.Abstraction;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Persistence.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly BlogSMDbContext _blogSMDbContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(BlogSMDbContext blogSMDbContext)
    {
        _blogSMDbContext = blogSMDbContext;
        _dbSet = blogSMDbContext.Set<T>();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Delete(T entity)
    {
       _dbSet.Remove(entity);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
       return await _dbSet.FindAsync(id);
    }

    public virtual async Task SaveAsync()
    {
        await _blogSMDbContext.SaveChangesAsync();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
