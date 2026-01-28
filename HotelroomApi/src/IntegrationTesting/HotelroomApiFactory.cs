using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Api.IntegrationTests;

public sealed class HotelroomApiFactory : WebApplicationFactory<Api.Program>
{
    public string DatabaseName { get; } = $"hotelroom_it_{Guid.NewGuid():N}";

    public string ConnectionString =>
        $@"Server=(localdb)\MSSQLLocalDB;Database={DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=True;";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", ConnectionString)
            });
        });
    }
}
