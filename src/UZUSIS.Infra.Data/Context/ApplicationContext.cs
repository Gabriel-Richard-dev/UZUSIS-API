using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UZUSIS.Domain.Abstractions;
using UZUSIS.Domain.Contracts;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Infra.Data.Context;

public class ApplicationContext : DbContext, IUnitOfWork
{
    public ApplicationContext(DbContextOptions options) : base(options) { }

    public DbSet<Administrador> Administradores { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Carrinho> Carrinhos { get; set; }

    public async Task<bool> Commit() => await SaveChangesAsync() > 0;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

   
} 