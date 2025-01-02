namespace EmployeeManagementSystem.Domain.Services.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetById(int id);
    IQueryable<TEntity> GetAll();
    Task<TEntity?> Add(TEntity entity);
    Task<TEntity?> Update(int? id, TEntity entity);
    Task Delete(int id);
}
