using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Infrastructure.Persistence.Configurations;

public sealed class ReservationClientConfiguration : IEntityTypeConfiguration<ReservationClient>
{
    public void Configure(EntityTypeBuilder<ReservationClient> builder)
    {
        builder.ToTable("tblReservationClient");

        builder.HasKey(x => new { x.ReservationId, x.FirstName, x.LastName });

        builder.Property(x => x.ReservationId)
               .HasColumnName("reservationId")
               .IsRequired();

        builder.Property(x => x.FirstName)
               .HasColumnName("firstName")
               .HasColumnType("char(50)")
               .IsFixedLength()
               .IsRequired();

        builder.Property(x => x.LastName)
               .HasColumnName("lastName")
               .HasColumnType("char(50)")
               .IsFixedLength()
               .IsRequired();

        builder.Property(x => x.BirthDate)
               .HasColumnName("birthDate")
               .HasColumnType("date")
               .IsRequired();

        builder.HasOne(x => x.Reservation)
               .WithMany(x => x.Clients)
               .HasForeignKey(x => x.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
