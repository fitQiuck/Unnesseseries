using Auth.DataAccess.AppDbContexts;
using Auth.DataAccess.Interface;
using Auth.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.DataAccess.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : Auditable
{
    private readonly AppDbContext dbContext;
    private readonly DbSet<T> dbSet;

    public GenericRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
        dbSet = dbContext.Set<T>();
    }

    public async ValueTask<T> CreateAsync(T entity) =>
        (await dbContext.AddAsync(entity)).Entity;

    public IQueryable<T> GetAll(Expression<Func<T, bool>> expression, string[] includes = null)
    {
        IQueryable<T> query = expression == null ? dbSet.Where(item => item.DeletedBy == null) : dbSet.Where(item => item.DeletedBy == null).Where(expression);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                if (!string.IsNullOrEmpty(include))
                    query = query.Include(include);
            }
        }
        return query;
    }

    public async ValueTask<T> GetAsync(Expression<Func<T, bool>> expression, string[] includes = null)
        => await GetAll(expression, includes).FirstOrDefaultAsync();

    public T Update(T entity)
        => dbSet.Update(entity).Entity;

    public async ValueTask SaveChangesAsync()
        => await dbContext.SaveChangesAsync();

    public async ValueTask<bool> DeleteAsync(T entity)
    {
        if (entity == null)
            return false;

        dbSet.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }
    public async ValueTask<bool> DeleteById(Guid id)
    {
        var entity = await dbSet.FindAsync(id);
        if (entity == null)
            return false;

        dbSet.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
