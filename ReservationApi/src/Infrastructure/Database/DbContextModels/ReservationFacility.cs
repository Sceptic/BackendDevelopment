using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Infrastructure.Persistence.Configurations;

public sealed class ReservationFacilityConfiguration : IEntityTypeConfiguration<ReservationFacility>
{
    public void Configure(EntityTypeBuilder<ReservationFacility> builder)
    {
        builder.ToTable("tblReservationFacility");

        builder.HasKey(x => new { x.ReservationId, x.Facility });

        builder.Property(x => x.ReservationId)
               .HasColumnName("reservationId")
               .IsRequired();

        builder.Property(x => x.Facility)
               .HasColumnName("facility")
               .HasColumnType("char(50)")
               .IsFixedLength()
               .IsRequired();

        builder.Property(x => x.FacilityDiscount)
               .HasColumnName("facilityDiscount")
               .HasPrecision(3, 2)
               .IsRequired();

        builder.HasOne(x => x.Reservation)
               .WithMany(x => x.Facilities)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
