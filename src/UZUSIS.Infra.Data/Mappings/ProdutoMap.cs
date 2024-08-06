using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Infra.Data.Mappings;

public class ProdutoMap : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .HasColumnType("VARCHAR(120)");
        builder.Property(c => c.Preco);
        builder.Property(c => c.Categoria)
            .HasColumnType("VARCHAR(2000)");

        builder.Property(c => c.Quantidade);
        
        builder.Property(c => c.CriadoEm);
        builder.Property(c => c.AtualizadoEm);

    }
}