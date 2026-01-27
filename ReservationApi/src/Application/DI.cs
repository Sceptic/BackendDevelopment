using Application.Abstractions.Reservations;
using Application.Reservations;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IReservationReadService, ReservationReadService>();
        services.AddScoped<IReservationCommandService, ReservationCommandService>();
        services.AddScoped<IReservationAvailabilityChecker, ReservationAvailabilityChecker>();
        services.AddScoped<IReservationExternalPolicy, ReservationExternalPolicy>();

        return services;
    }
}
