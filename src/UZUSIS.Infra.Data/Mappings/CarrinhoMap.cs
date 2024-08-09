using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Infra.Data.Mappings;

public class CarrinhoMap : IEntityTypeConfiguration<Carrinho>
{
    public void Configure(EntityTypeBuilder<Carrinho> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ClienteId);

        builder.HasOne(c => c.Cliente)
            .WithOne(c => c.Carrinho)
            .HasForeignKey<Carrinho>(c => c.Id);

        builder.HasMany(c => c.Pedidos)
            .WithOne(c => c.Carrinho);
    }
}