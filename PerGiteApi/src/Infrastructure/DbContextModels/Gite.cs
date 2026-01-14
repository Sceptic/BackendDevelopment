using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class GiteConfiguration : IEntityTypeConfiguration<Gite>
{
    public void Configure(EntityTypeBuilder<Gite> builder)
    {
        builder.ToTable("tblGite");

        builder.HasKey(x => x.GiteId);

        builder.Property(x => x.GiteId)
               .HasColumnName("giteId");

        builder.Property(x => x.GiteNumber)
               .HasColumnName("giteNumber")
               .IsRequired();

        builder.HasIndex(x => x.GiteNumber)
               .IsUnique();

        builder.Property(x => x.GitePrice)
               .HasColumnName("gitePrice")
               .HasPrecision(5, 2)
               .IsRequired();

        builder.Property(x => x.IsAvailable)
               .HasColumnName("isAvailable")
               .IsRequired();

        builder.Property(x => x.GiteAddress)
               .HasColumnName("giteAddress")
               .HasColumnType("char(100)")
               .IsFixedLength()
               .IsRequired();

        builder.Property(x => x.CapacityMin)
               .HasColumnName("capacityMin")
               .IsRequired();

        builder.Property(x => x.CapacityMax)
               .HasColumnName("capacityMax")
               .IsRequired();

        builder.HasOne(x => x.Amenities)
               .WithOne(x => x.Gite)
               .HasForeignKey<GiteAmenities>(x => x.GiteId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Beds)
               .WithOne(x => x.Gite)
               .HasForeignKey(x => x.GiteId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
