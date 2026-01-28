using IntegrationTesting.Helpers;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api.IntegrationTests;

public sealed class ReservationApiFactory : WebApplicationFactory<Api.Program>
{
    public string DatabaseName { get; } = $"reservation_it_{Guid.NewGuid():N}";

    public string ConnectionString =>
        $@"Server=(localdb)\MSSQLLocalDB;Database={DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=True;";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ReservationDbContext>>();
            services.RemoveAll<ReservationDbContext>();

            services.AddDbContext<ReservationDbContext>(o =>
                o.UseSqlServer(ConnectionString, sql =>
                {
                    sql.EnableRetryOnFailure();
                    sql.MigrationsAssembly(typeof(ReservationDbContext).Assembly.FullName);
                }));

            services.RemoveAll<Application.Abstractions.IGiteReadClient>();
            services.RemoveAll<Application.Abstractions.IHotelroomReadClient>();
            services.RemoveAll<Application.Abstractions.ICampingReadClient>();
            services.RemoveAll<Application.Abstractions.IRestaurantReadClient>();

            services.AddSingleton<Application.Abstractions.IGiteReadClient, StubGiteReadClient>();
            services.AddSingleton<Application.Abstractions.IHotelroomReadClient, StubHotelroomReadClient>();
            services.AddSingleton<Application.Abstractions.ICampingReadClient, StubCampingReadClient>();
            services.AddSingleton<Application.Abstractions.IRestaurantReadClient, StubRestaurantReadClient>();
        });
    }
}
