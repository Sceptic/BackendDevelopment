using Application.Abstractions;
using Application.Gites.ReadQueries;
using Application.Gites.WriteQueries;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationDi
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<GiteReadingService>();
            services.AddScoped<GiteWritingService>();

            return services;
        }
    }
}