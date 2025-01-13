using EmployeeManagementSystem.Domain.Services.DependencyResolver;
using EmployeeManagementSystem.Domain.Services.Repositories;
using EmployeeManagementSystem.Domain.Services.UnitOfWorks;
using EmployeeManagementSystem.Infrastructure.Repositories;
using EmployeeManagementSystem.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.Infrastructure.DependencyResolver;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseContext(configuration);
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
