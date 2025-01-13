using EmployeeManagementSystem.Domain.Services.Repositories;

namespace EmployeeManagementSystem.Domain.Services.UnitOfWorks
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
        IRepository<T> Repository<T>() where T : class;
    }
}