using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Infrastructure.Persistence.Configurations;

public sealed class ReservationRestaurantConfiguration : IEntityTypeConfiguration<ReservationRestaurant>
{
    public void Configure(EntityTypeBuilder<ReservationRestaurant> builder)
    {
        builder.ToTable("tblReservationRestaurant");

        builder.HasKey(x => x.ReservationRestaurantId);

        builder.Property(x => x.ReservationRestaurantId)
               .HasColumnName("reservationRestaurantId");

        builder.Property(x => x.ReservationId)
               .HasColumnName("reservationId")
               .IsRequired();

        builder.Property(x => x.TableId)
               .HasColumnName("tableId")
               .IsRequired();

        builder.Property(x => x.TableReservationStart)
               .HasColumnName("tableReservationStart")
               .IsRequired();

        builder.Property(x => x.TableReservationEnd)
               .HasColumnName("tableReservationEnd")
               .IsRequired();

        builder.Property(x => x.TableBill)
               .HasColumnName("tableBill")
               .HasPrecision(5, 2)
               .IsRequired();

        builder.Property(x => x.TableDiscount)
               .HasColumnName("tableDiscount")
               .HasPrecision(3, 2)
               .IsRequired();

        builder.HasOne(x => x.Reservation)
               .WithMany(x => x.Restaurants)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
