using Dokkan.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dokkan.Api.Persistence.EntitiesConfigurations;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(250);

        builder.HasIndex(x => new { x.Name, x.BrandId }).IsUnique();


        builder.Property(x => x.Description)
            .HasMaxLength(1500);

        builder.Property(x => x.BasePrice)
            .HasPrecision(18, 2);
    }
}
