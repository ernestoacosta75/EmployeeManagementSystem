using EmployeeManagementSystem.Application.Services.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.Application.Services.DependencyResolver;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddAutoMapper(typeof(AutoMapperProfiles));
    }
}
