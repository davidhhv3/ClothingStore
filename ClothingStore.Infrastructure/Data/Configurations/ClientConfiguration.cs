using ClothingStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStore.Infrastructure.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Cliente");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                  .HasColumnName("idCliente");

            builder.Property(e => e.Country)
                  .HasColumnName("Pais_idPais")
                  .IsRequired();

            builder.Property(e => e.Name)
                  .HasColumnName("Nombre")
                  .IsRequired()
                  .HasMaxLength(20);

            builder.Property(e => e.LastName)
                  .HasColumnName("Apellido")
                  .IsRequired()
                  .HasMaxLength(20);

            builder.Property(e => e.Age)
                  .HasColumnName("Edad")
                  .IsRequired();


            builder.Property(e => e.IdentificationNumber)
                  .HasColumnName("Cedula")
                  .IsRequired();

        }
    }
}
