using EmployeeManagementSystem.Domain.Services.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementSystem.Domain.Services.DependencyResolver;

public static class ServiceCollectionExtension
{
    public static void AddDatabaseContext(this IServiceCollection services,
                                          IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("No connection string found in configuration.");

        services.AddDbContextFactory<AppDbContext>(options =>
                    options
                        .UseSqlServer(connectionString, sqlServer =>
                        {
                            sqlServer.UseNetTopologySuite();
                        })
                        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Warning),
                ServiceLifetime.Scoped);
    }
}
