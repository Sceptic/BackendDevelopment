using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Infrastructure.Persistence.Configurations;

public sealed class ReservationHotelroomConfiguration : IEntityTypeConfiguration<ReservationHotelroom>
{
    public void Configure(EntityTypeBuilder<ReservationHotelroom> builder)
    {
        builder.ToTable("tblReservationHotelroom");

        builder.HasKey(x => new { x.ReservationId, x.RoomId });

        builder.Property(x => x.ReservationId)
               .HasColumnName("reservationId")
               .IsRequired();

        builder.Property(x => x.RoomId)
               .HasColumnName("roomId")
               .IsRequired();

        builder.Property(x => x.HotelroomDiscount)
               .HasColumnName("hotelroomDiscount")
               .HasPrecision(3, 2)
               .IsRequired();

        builder.HasOne(x => x.Reservation)
               .WithMany(x => x.Hotelrooms)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
