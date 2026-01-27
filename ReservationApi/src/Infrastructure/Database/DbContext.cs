using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class ReservationDbContext : DbContext
{
    public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }

    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<ReservationClient> ReservationClients { get; set; } = null!;
    public DbSet<ReservationGite> ReservationGites { get; set; } = null!;
    public DbSet<ReservationHotelroom> ReservationHotelrooms { get; set; } = null!;
    public DbSet<ReservationCamping> ReservationCampings { get; set; } = null!;
    public DbSet<ReservationRestaurant> ReservationRestaurants { get; set; } = null!;
    public DbSet<ReservationFacility> ReservationFacilities { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReservationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
