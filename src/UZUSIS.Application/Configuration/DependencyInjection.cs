using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UZUSIS.Application.Notification;
using UZUSIS.Infra.Data.Configuration;
using UZUSIS.Infra.Data.Context;
using Pomelo.EntityFrameworkCore.MySql;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Services;

namespace UZUSIS.Application.Configuration;

public static class DependencyInjection
{
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        
    }



    public static void ConfigurarDependencias(this IServiceCollection services)
    {
        services.AdicionarDependenciasRepository();

        services
            .AddScoped<INotificator, Notificator>();

        services
            .AddScoped<IAdministradorService, AdministradorService>();
    }
}