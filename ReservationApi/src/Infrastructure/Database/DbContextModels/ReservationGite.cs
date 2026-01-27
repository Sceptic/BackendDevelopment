using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Infrastructure.Persistence.Configurations;

public sealed class ReservationGiteConfiguration : IEntityTypeConfiguration<ReservationGite>
{
    public void Configure(EntityTypeBuilder<ReservationGite> builder)
    {
        builder.ToTable("tblReservationGite");

        builder.HasKey(x => new { x.ReservationId, x.GiteId });

        builder.Property(x => x.ReservationId)
               .HasColumnName("reservationId")
               .IsRequired();

        builder.Property(x => x.GiteId)
               .HasColumnName("giteId")
               .IsRequired();

        builder.Property(x => x.GiteDiscount)
               .HasColumnName("giteDiscount")
               .HasPrecision(3, 2)
               .IsRequired();

        builder.HasOne(x => x.Reservation)
               .WithMany(x => x.Gites)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
