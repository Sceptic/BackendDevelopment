using Application.Abstractions;
using Application.Abstractions.Persistence;
using Infrastructure.ExternalApi.Configurator;
using Infrastructure.ExternalApi.Gites;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //DB Connection Registration
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

        services.AddDbContext<ReservationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure();
                sql.MigrationsAssembly(typeof(ReservationDbContext).Assembly.FullName);
            });
        });

        //API Registration

        services.Configure<GiteApiOptions>(configuration.GetSection("ExternalApis:Gite"));
        services.Configure<CampingApiOptions>(configuration.GetSection("ExternalApis:Camping"));
        services.Configure<HotelroomApiOptions>(configuration.GetSection("ExternalApis:Hotel"));
        services.Configure<RestaurantApiOptions>(configuration.GetSection("ExternalApis:Restaurant"));

        services.AddHttpClient<IGiteReadClient, GiteClient>((sp, http) =>
        {
            var opt = sp.GetRequiredService<IOptions<GiteApiOptions>>().Value;
            ExternalApiHttpClientConfig.ConfigureJsonApi(http, opt.BaseUrl, opt.TimeoutSeconds);
        });

        services.AddHttpClient<ICampingReadClient, CampingClient>((sp, http) =>
        {
            var opt = sp.GetRequiredService<IOptions<CampingApiOptions>>().Value;
            ExternalApiHttpClientConfig.ConfigureJsonApi(http, opt.BaseUrl, opt.TimeoutSeconds);
        });

        services.AddHttpClient<IRestaurantReadClient, RestaurantClient>((sp, http) =>
        {
            var opt = sp.GetRequiredService<IOptions<RestaurantApiOptions>>().Value;
            ExternalApiHttpClientConfig.ConfigureJsonApi(http, opt.BaseUrl, opt.TimeoutSeconds);
        });

        services.AddHttpClient<IHotelroomReadClient, HotelroomClient>((sp, http) =>
        {
            var opt = sp.GetRequiredService<IOptions<HotelroomApiOptions>>().Value;
            ExternalApiHttpClientConfig.ConfigureJsonApi(http, opt.BaseUrl, opt.TimeoutSeconds);
        });

        //Application Port-Interface Implementations

        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReservationConflictQueries, ReservationConflictQueries>();

        return services;
    }
}
