using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UZUSIS.Domain.Abstractions;
using UZUSIS.Domain.Contracts;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Infra.Data.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected DbSet<Administrador> Administradores { get; set; }
    protected DbSet<Cliente> Clientes { get; set; }
    protected DbSet<Produto> Produtos { get; set; }
    protected DbSet<Carrinho> Carrinhos { get; set; }

    public async Task<bool> Commit() => await SaveChangesAsync() > 0;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
} 