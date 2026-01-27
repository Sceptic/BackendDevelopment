using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("tblReservation");

        builder.HasKey(x => x.ReservationId);

        builder.Property(x => x.ReservationId)
               .HasColumnName("reservationId");

        builder.Property(x => x.AccountId)
               .HasColumnName("accountId")
               .IsRequired();

        builder.Property(x => x.ReservationStatus)
               .HasColumnName("reservationStatus")
               .HasColumnType("text")
               .IsRequired();

        builder.Property(x => x.PaymentStatus)
               .HasColumnName("paymentStatus")
               .HasColumnType("text")
               .IsRequired();

        builder.Property(x => x.ReservationPrice)
               .HasColumnName("reservationPrice")
               .HasPrecision(6, 2)
               .IsRequired();

        builder.Property(x => x.Discount)
               .HasColumnName("discount")
               .HasPrecision(3, 2)
               .IsRequired();

        builder.Property(x => x.TouristTarif)
               .HasColumnName("touristTarif")
               .HasPrecision(3, 2)
               .IsRequired();

        builder.Property(x => x.ReservationStart)
               .HasColumnName("reservationStart")
               .IsRequired();

        builder.Property(x => x.ReservationEnd)
               .HasColumnName("reservationEnd")
               .IsRequired();

        builder.HasMany(x => x.Clients)
               .WithOne(x => x.Reservation)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Gites)
               .WithOne(x => x.Reservation)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Hotelrooms)
               .WithOne(x => x.Reservation)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Campings)
               .WithOne(x => x.Reservation)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Restaurants)
               .WithOne(x => x.Reservation)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Facilities)
               .WithOne(x => x.Reservation)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Vehicles)
               .WithOne(x => x.Reservation)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
