using D = Application.DBSeeder;
using Application.Interfaces.Services;
using Application.Services;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Mappings;
using EmployeeDirectory.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Application.DBSeeder;
using Application.DTO.RequestDTOs;

namespace Application.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<ILocationService, LocationService>();
        services.AddTransient<IDepartmentService, DepartmentService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<D.DBSeeder>();
        //services.AddTransient<AdminSettingsDTO>();

        services.AddAutoMapper(typeof(EmployeeMappingProfile).Assembly);
        services.AddAutoMapper(typeof(DepartmentMappingProfile).Assembly);
        services.AddAutoMapper(typeof(LocationMappingProfile).Assembly);
        services.AddAutoMapper(typeof(RoleMappingProfile).Assembly);
        services.AddAutoMapper(typeof(ProjectMappingProfile).Assembly);

        return services;
    }

    public static async Task AddSeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var seeder = scope.ServiceProvider.GetRequiredService<D.DBSeeder>();

        await seeder.SeedAdminAsync(scope.ServiceProvider);
    }

}

