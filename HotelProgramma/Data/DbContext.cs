using HotelProgramma.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelProgramma.Data
{
    public class DbMarconnes : DbContext
    {
        public DbSet<Account> tblAccount { get; set; }
        public DbSet<Gite> tblGite { get; set; }
        public DbSet<HotelRoom> tblHotelRoom { get; set; }
        public DbSet<HotelRoomBed> tblHotelRoomBed { get; set; }
        public DbSet<HotelRoomAmenities> tblHotelRoomAmenity { get; set; }
        public DbSet<Reservation> tblReservation { get; set; }
        public DbSet<ReservationClient> tblReservationClient { get; set; }
        public DbSet<ReservationGite> tblReservationGite { get; set; }
        public DbSet<ReservationHotel> tblReservationHotel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connection = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connection);        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===== ===== USER TABLES ===== =====

            // ACCOUNT
            modelBuilder.Entity<Account>()
                .HasKey(x => x.AccountId);

            // ===== ===== SERVICES TABLES ===== =====

            // GITE
            modelBuilder.Entity<Gite>()
                .HasKey(x => x.GiteNumber);

            modelBuilder.Entity<Gite>()
                .Property(x => x.GiteAddress)
                .HasMaxLength(100)
                .IsRequired();

            // HOTELROOM
            modelBuilder.Entity<HotelRoom>()
                .HasKey(x => x.RoomNumber);

            // HOTELROOMBED
            modelBuilder.Entity<HotelRoomBed>()
                .HasKey(x => x.RoomNumber);

            modelBuilder.Entity<HotelRoomBed>()
                .HasOne(x => x.Room)
                .WithOne(x => x.Bed)
                .HasForeignKey<HotelRoomBed>(x => x.RoomNumber)
                .OnDelete(DeleteBehavior.Cascade);

            // HOTELROOMAMENITIES
            modelBuilder.Entity<HotelRoomAmenities>()
                .HasKey(x => x.RoomNumber);

            modelBuilder.Entity<HotelRoomAmenities>()
                .HasOne(x => x.Room)
                .WithOne(x => x.Amenities)
                .HasForeignKey<HotelRoomAmenities>(x => x.RoomNumber)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== ===== RESERVATION TABLES ===== =====

            // RESERVATION
            modelBuilder.Entity<Reservation>()
                .HasKey(x => x.ReservationId);

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.Account)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.AccountId);

            // RESERVATIONCLIENT
            modelBuilder.Entity<ReservationClient>()
                .HasKey(x => new { x.ReservationId, x.Firstname, x.Lastname });

            modelBuilder.Entity<ReservationClient>()
                .HasOne(x => x.Reservation)
                .WithMany(x => x.ReservationClient)
                .HasForeignKey(x => x.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // RESERVATIONGITE
            modelBuilder.Entity<ReservationGite>()
                .HasKey(x => new { x.ReservationId, x.GiteNumber });

            modelBuilder.Entity<ReservationGite>()
                .HasOne(x => x.Reservation)
                .WithMany(x => x.ReservationGite)
                .HasForeignKey(x => x.ReservationId);

            modelBuilder.Entity<ReservationGite>()
                .HasOne(x => x.Gite)
                .WithMany(x => x.ReservationGites)
                .HasForeignKey(x => x.GiteNumber)
                .OnDelete(DeleteBehavior.Restrict);

            // RESERVATIONHOTEL
            modelBuilder.Entity<ReservationHotel>()
                .HasKey(x => new { x.ReservationId, x.RoomNumber });

            modelBuilder.Entity<ReservationHotel>()
                .HasOne(x => x.Reservation)
                .WithMany(x => x.ReservationHotel)
                .HasForeignKey(x => x.ReservationId);

            modelBuilder.Entity<ReservationHotel>()
                .HasOne(x => x.Room)
                .WithMany(x => x.ReservationHotels)
                .HasForeignKey(x => x.RoomNumber)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
