using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ReservationCampingConfiguration : IEntityTypeConfiguration<ReservationCamping>
{
    public void Configure(EntityTypeBuilder<ReservationCamping> builder)
    {
        builder.ToTable("tblReservationCamping");

        builder.HasKey(x => new { x.ReservationId, x.CampingId });

        builder.Property(x => x.ReservationId)
               .HasColumnName("reservationId")
               .IsRequired();

        builder.Property(x => x.CampingId)
               .HasColumnName("campingId")
               .IsRequired();

        builder.Property(x => x.CampingDiscount)
               .HasColumnName("campingDiscount")
               .HasPrecision(3, 2)
               .IsRequired();

        builder.HasOne(x => x.Reservation)
               .WithMany(x => x.Campings)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
