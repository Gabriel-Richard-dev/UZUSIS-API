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
    public DbSet<Tamanho> Tamanhos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Foto> Fotos { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Carrinho> Carrinhos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<ConfirmacaoEmail> ConfirmacoesDeEmails { get; set; }

    public async Task<bool> Commit() => await SaveChangesAsync() > 0;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

   
} 