using System.Net.Http.Headers;

namespace Infrastructure.ExternalApi.Configurator;

//Provides all necessary configuration mechanics to register API Clients in the DI Module.
internal static class ExternalApiHttpClientConfig
{
    internal static void ConfigureJsonApi(HttpClient http, string baseUrl, int timeoutSeconds)
    {
        http.BaseAddress = new Uri(baseUrl);
        http.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        http.DefaultRequestHeaders.Accept.Clear();
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}

public sealed class GiteApiOptions 
{ 
    public string BaseUrl { get; init; } = default!; 
    public int TimeoutSeconds { get; init; } = 15; 
}

public sealed class CampingApiOptions 
{ 
    public string BaseUrl { get; init; } = default!; 
    public int TimeoutSeconds { get; init; } = 15; 
}

public sealed class HotelroomApiOptions 
{ 
    public string BaseUrl { get; init; } = default!; 
    public int TimeoutSeconds { get; init; } = 15; 
}

public sealed class RestaurantApiOptions 
{ 
    public string BaseUrl { get; init; } = default!; 
    public int TimeoutSeconds { get; init; } = 15; 
}
