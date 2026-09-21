using Application.Interfaces.Repositories;
using EmployeeDirectory.Application.Repositories.Interfaces;
using EmployeeDirectory.Application.Repositories.Repositories;
using EmployeeDirectory.Infrastructure.Repository;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class RepositoryServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>(); 
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IEmployeeProjectRepository, EmployeeProjectRepository>();
        services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
        services.AddScoped<ILocationDepartmentRepository, LocationDepartmentRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

