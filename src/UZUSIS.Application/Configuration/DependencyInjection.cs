using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UZUSIS.Application.Notification;
using UZUSIS.Infra.Data.Configuration;
using UZUSIS.Infra.Data.Context;

namespace UZUSIS.Application.Configuration;

public static class DependencyInjection
{
    public static void AdicionarDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
        
        
    }

    public static void ConfigurarDependencias(this IServiceCollection services)
    {
        services.AdicionarDependenciasRepository();

        services.AddScoped<INotificator, Notificator>();

        
    }
}