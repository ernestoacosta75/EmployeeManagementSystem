using EmployeeManagementSystem.Application.Services.Mappers;
using EmployeeManagementSystem.Application.Services.Profiles;
using EmployeeManagementSystem.Application.Services.UserAccount;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.Application.Services.DependencyResolver;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        //services.AddMemoryCache();
        services.AddAutoMapper(typeof(AutoMapperProfiles));
        services.AddTransient<IUserAccountService, UserAccountService>();
        services.AddScoped(typeof(EntityDtoMapper<,>));
    }
}
