using ClothingStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStore.Infrastructure.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Pais");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                  .HasColumnName("IdPais");

            builder.Property(e => e.Name)
                  .HasColumnName("Nombre")
                  .IsRequired()
                  .HasMaxLength(20);
        }
    }

}
