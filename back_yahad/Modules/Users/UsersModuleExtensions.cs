using back_yahad.Modules.Users.Endpoints;
using back_yahad.Modules.Users.Repositories;

namespace back_yahad.Modules.Users;

public static class UsersModuleExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, EfRoleRepository>();
        services.AddScoped<IUsuarioRepository, EfUsuarioRepository>();
        return services;    
    }

    public static IEndpointRouteBuilder MapUsersModule(this IEndpointRouteBuilder app)
    {
        app.MapRolesEndpoints();
        app.MapUsuariosEndpoints();
        return app;
    }
}
