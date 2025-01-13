using EmployeeManagementSystem.Domain.Services.DatabaseContext;
using EmployeeManagementSystem.Domain.Services.Repositories;
using EmployeeManagementSystem.Domain.Services.UnitOfWorks;
using EmployeeManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _dbContext => _dbContextFactory.CreateDbContext();
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public UnitOfWork(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_dbContextFactory);
        }
    }
}
