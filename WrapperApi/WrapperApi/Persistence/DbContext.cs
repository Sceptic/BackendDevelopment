using Microsoft.EntityFrameworkCore;
using WrapperApi.Contracts;

namespace WrapperApi.Persistence;

public sealed class PlatformDbContext : DbContext
{
    public PlatformDbContext(DbContextOptions<PlatformDbContext> options) : base(options) { }

    public DbSet<PlatformReservationEntity> PlatformReservations => Set<PlatformReservationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlatformReservationEntity>(e =>
        {
            e.HasKey(x => x.PlatformReservationId);
            e.Property(x => x.Source).HasConversion<int>();

            e.Property(x => x.SourceReservationId).IsRequired().HasMaxLength(128);
            e.Property(x => x.IdempotencyKey).HasMaxLength(128);

            e.HasIndex(x => x.IdempotencyKey).IsUnique().HasFilter("[IdempotencyKey] IS NOT NULL");
        });
    }
}

public sealed class PlatformReservationEntity
{
    public int PlatformReservationId { get; set; }
    public SourceSystem Source { get; set; }
    public string SourceReservationId { get; set; } = default!;
    public string? IdempotencyKey { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Optioneel cache
    public int? AccountId { get; set; }
    public string? AccommodationId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int? Guests { get; set; }
}
