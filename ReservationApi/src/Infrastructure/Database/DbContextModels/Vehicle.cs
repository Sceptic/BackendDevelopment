using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Infrastructure.Persistence.Configurations;

public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("tblVehicle");

        builder.HasKey(x => new { x.ReservationId, x.RegistrationPlate });

        builder.Property(x => x.ReservationId)
               .HasColumnName("reservationId")
               .IsRequired();

        builder.Property(x => x.RegistrationPlate)
               .HasColumnName("registrationPlate")
               .HasColumnType("char(50)")
               .IsFixedLength()
               .IsRequired();

        builder.HasOne(x => x.Reservation)
               .WithMany(x => x.Vehicles)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
