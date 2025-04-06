using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TesteMobi2Buy.Domain.Entities;

namespace TesteMobi2Buy.Infrastructure.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Cep)
            .IsRequired()
            .HasMaxLength(9);

        builder.Property(c => c.Logradouro)
            .HasMaxLength(150);

        builder.Property(c => c.Bairro)
            .HasMaxLength(100);

        builder.Property(c => c.Cidade)
            .HasMaxLength(100);

        builder.Property(c => c.Estado)
            .HasMaxLength(2);

        builder.HasIndex(c => c.Email)
            .IsUnique();
    }
}
