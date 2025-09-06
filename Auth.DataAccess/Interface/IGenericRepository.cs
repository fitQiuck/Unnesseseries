using Auth.Domain.Common;
using System.Linq.Expressions;

namespace Auth.DataAccess.Interface;

public interface IGenericRepository<T> where T : Auditable
{
    ValueTask<T> CreateAsync(T entity);
    ValueTask<bool> DeleteAsync(T entity);
    ValueTask<bool> DeleteById(Guid id);
    IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null, string[] includes = null);
    ValueTask<T> GetAsync(Expression<Func<T, bool>> expression, string[] includes = null);
    T Update(T entity);
    public ValueTask SaveChangesAsync();
}
