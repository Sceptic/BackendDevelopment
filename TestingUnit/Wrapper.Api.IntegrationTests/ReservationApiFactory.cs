using Application.Abstractions.Reservations;
using Infrastructure.Database; // adjust to your real DbContext namespace
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ReservationApi.IntegrationTests;

public sealed class ReservationApiFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _conn;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1) Replace DbContext with SQLite in-memory
            _conn = new SqliteConnection("DataSource=:memory:");
            _conn.Open();

            // remove existing DbContext registration
            var dbDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<ReservationDbContext>));
            if (dbDescriptor is not null) services.Remove(dbDescriptor);

            services.AddDbContext<ReservationDbContext>(o =>
                o.UseSqlite(_conn));

            // ensure schema exists
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
            db.Database.EnsureCreated();

            // 2) Replace external policy with stub
            var ext = services.SingleOrDefault(d => d.ServiceType == typeof(IReservationExternalPolicy));
            if (ext is not null) services.Remove(ext);
            services.AddSingleton<IReservationExternalPolicy, NoOpExternalPolicy>();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _conn?.Dispose();
            _conn = null;
        }
    }

    private sealed class NoOpExternalPolicy : IReservationExternalPolicy
    {
        public Task ApplyAsync(Domain.Models.Reservation reservation, CancellationToken ct) => Task.CompletedTask;
    }
}