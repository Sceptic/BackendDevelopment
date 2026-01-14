using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public sealed class GiteDbContext : DbContext
    {
        public GiteDbContext(DbContextOptions<GiteDbContext> options) : base(options) { }

        public DbSet<Gite> Gites { get; set; } = null!;
        public DbSet<GiteAmenities> GiteAmenities { get; set; } = null!;
        public DbSet<GiteBed> GiteBeds { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GiteDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}

