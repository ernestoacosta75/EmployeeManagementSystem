using EmployeeManagementSystem.Domain.Services.Repositories;
using EmployeeManagementSystem.Domain.Services.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _appDbContext = dbContextFactory.CreateDbContext();
        _dbSet = _appDbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<TEntity?> Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            var addedEntity = await _dbSet.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();

            return addedEntity.Entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding entity: {ex.Message}");
            throw;
        }
    }

    public async Task<TEntity?> Update(int? id, TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(id);

        var existingEntity = await _dbSet.FindAsync(id);

        if (existingEntity == null)
        {
            return null;
        }

        _appDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _appDbContext.SaveChangesAsync();

        return existingEntity;
    }

    public async Task Delete(int id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var entityToDelete = await _dbSet.FindAsync(id);

        try
        {
            _dbSet.Remove(entityToDelete);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting entity with ID {id}: {ex.Message}");
            throw;
        }
    }
}
