using Application.Hotelrooms.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetAllHotelroomsQuery>();
        services.AddScoped<GetHotelroomByIdQuery>();

        return services;
    }
}
