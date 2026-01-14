using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class GiteAmenitiesConfiguration : IEntityTypeConfiguration<GiteAmenities>
{
    public void Configure(EntityTypeBuilder<GiteAmenities> builder)
    {
        builder.ToTable("tblGiteAmenities");

        builder.HasKey(x => x.GiteId);

        builder.Property(x => x.GiteId)
               .HasColumnName("giteId");

        builder.Property(x => x.Wifi).HasColumnName("wifi");
        builder.Property(x => x.Bath).HasColumnName("bath");
        builder.Property(x => x.Shower).HasColumnName("shower");
        builder.Property(x => x.HairDryer).HasColumnName("hairDryer");
        builder.Property(x => x.SmallChild).HasColumnName("smallChild");
        builder.Property(x => x.Toiletries).HasColumnName("toiletries");
        builder.Property(x => x.Desk).HasColumnName("desk");
        builder.Property(x => x.Chair).HasColumnName("chair");
        builder.Property(x => x.Balcony).HasColumnName("balcony");
        builder.Property(x => x.Sofa).HasColumnName("sofa");
        builder.Property(x => x.SofaBed).HasColumnName("sofaBed");
        builder.Property(x => x.MiniFridge).HasColumnName("miniFridge");
        builder.Property(x => x.Kettle).HasColumnName("kettle");
        builder.Property(x => x.Cuttlery).HasColumnName("cuttlery");
        builder.Property(x => x.EatingArea).HasColumnName("eatingArea");
        builder.Property(x => x.RoomService).HasColumnName("roomService");
    }
}
