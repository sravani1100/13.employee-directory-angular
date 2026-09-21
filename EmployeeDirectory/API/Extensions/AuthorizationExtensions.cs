namespace API.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApplicationAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy("EmployeeRead", policy =>
                policy.RequireAuthenticatedUser());
        });

        return services;
    }
}

