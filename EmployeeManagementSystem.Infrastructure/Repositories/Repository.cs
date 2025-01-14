using System.Linq.Expressions;
using EmployeeManagementSystem.Domain.Services.Repositories;
using EmployeeManagementSystem.Domain.Services.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<TEntity> _dbSet;

    //public Repository(IDbContextFactory<AppDbContext> dbContextFactory)
    //{
    //    _appDbContext = dbContextFactory.CreateDbContext();
    //    _dbSet = _appDbContext.Set<TEntity>();
    //}

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = _appDbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<TEntity?> Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var addedEntity = await _dbSet.AddAsync(entity);

        return addedEntity.Entity;
    }

    public async Task<TEntity?> Update(int? id, TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(id);

        var existingEntity = await _dbSet.FindAsync(id);

        if (existingEntity == null)
        {
            return null;
        }

        var entityUpdated = _dbSet.Update(entity);

        return entityUpdated.Entity ?? null;
    }

    public async Task Delete(int id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var entityToDelete = await _dbSet.FindAsync(id);

        try
        {
            if (entityToDelete != null) _dbSet.Remove(entityToDelete);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting entity with ID {id}: {ex.Message}");
            throw;
        }
    }
}
