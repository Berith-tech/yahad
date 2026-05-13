using back_yahad.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace back_yahad.Infrastructure.DependencyInjection;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "ConnectionString 'Default' não configurada. Crie 'appsettings.Local.json' a partir de 'appsettings.Local.example.json' ou defina a variável de ambiente ConnectionStrings__Default.");

        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
        return services;
    }
}
