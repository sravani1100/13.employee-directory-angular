using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EmployeeDirectory.Common.DB_Helper;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../API"))
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                                            .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                                            .Options;

        return new AppDbContext(options);
    }
}
