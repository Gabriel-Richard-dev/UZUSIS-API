using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Infra.Data.Mappings;

public class CarrinhoMap : IEntityTypeConfiguration<Carrinho>
{
    public void Configure(EntityTypeBuilder<Carrinho> builder)
    {
        builder.ToTable("Carrinho");
        
        
        builder.HasKey(c => c.Id);

        builder.HasOne<Cliente>()
            .WithOne(c => c.Carrinho)
            .HasForeignKey<Cliente>(c => c.CarrinhoId);
        
        builder.HasMany(c => c.Pedidos)
            .WithOne(c => c.Carrinho);
    }
}