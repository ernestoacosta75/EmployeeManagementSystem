using System.Linq.Expressions;

namespace EmployeeManagementSystem.Domain.Services.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetById(int id);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> GetAll();
    Task<TEntity?> Add(TEntity entity);
    Task<TEntity?> Update(int? id, TEntity entity);
    Task Delete(int id);
}
