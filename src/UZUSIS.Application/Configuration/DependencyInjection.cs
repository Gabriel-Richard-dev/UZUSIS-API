﻿using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UZUSIS.Application.Notification;
using UZUSIS.Infra.Data.Configuration;
using UZUSIS.Infra.Data.Context;
using Pomelo.EntityFrameworkCore.MySql;
using ScottBrady91.AspNetCore.Identity;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Administrador;
using UZUSIS.Application.Services;
using UZUSIS.Core.Settings;
using UZUSIS.Domain.Entities;

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

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    }



    public static void ConfigurarDependencias(this IServiceCollection services)
    {
        services.AdicionarDependenciasRepository();

        services
            .AddScoped<INotificator, Notificator>();
        
        services
            .AddScoped<IPasswordHasher<Administrador>, Argon2PasswordHasher<Administrador>>();
        
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services
            .AddScoped<IAdministradorService, AdministradorService>()
            .AddScoped<IAdminAuthService, AdminAuthService>()
            .AddScoped<IProdutoService, ProdutoService>();
    }
}