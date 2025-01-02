using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Domain.Services.DatabaseContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
    }
}
