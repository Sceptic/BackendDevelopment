using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WrapperApi.Contracts;
using WrapperApi.Persistence;
using WrapperApi.Providers;
using WrapperApi.Storage;
using WrapperApi.Wrapper;
using static WrapperApi.Wrapper.DalApiClient;
// Aliases om dubbele namen te vermijden
using PlatformOrchestrator = WrapperApi.Orchestration.ReservationOrchestrator;

var builder = WebApplication.CreateBuilder(args);

// Config expliciet opbouwen (lokaal/azure friendly)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// ===== Services/config =====

// Connection string voor reservation_db (jij gebruikt dezelfde keynaam "PlatformDb")
builder.Services.AddDbContext<PlatformDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformDb")));

// Config section met base urls
builder.Services.Configure<ServiceUrls>(builder.Configuration.GetSection("Services"));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (open)
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Reservation DB access (nieuwe architectuur)
builder.Services.AddScoped<IReservationDb, ReservationDb>();

// Providers: alleen live lezen
builder.Services.AddScoped<ICatalogProvider, HotelProvider>();
builder.Services.AddScoped<ICatalogProvider, GiteProvider>();
builder.Services.AddScoped<ICatalogProvider, CampingProvider>();

// Orchestrator
builder.Services.AddScoped<PlatformOrchestrator>();

// ===== Typed HttpClients naar de services =====

builder.Services.AddHttpClient<AccountsApiClient>((sp, http) =>
{
    var urls = sp.GetRequiredService<IOptions<ServiceUrls>>().Value;
    http.BaseAddress = new Uri(urls.AccountsBaseUrl.TrimEnd('/') + "/");
});

builder.Services.AddHttpClient<HotelApiClient>((sp, http) =>
{
    var urls = sp.GetRequiredService<IOptions<ServiceUrls>>().Value;
    http.BaseAddress = new Uri(urls.HotelBaseUrl.TrimEnd('/') + "/");
});

builder.Services.AddHttpClient<GiteApiClient>((sp, http) =>
{
    var urls = sp.GetRequiredService<IOptions<ServiceUrls>>().Value;
    http.BaseAddress = new Uri(urls.GiteBaseUrl.TrimEnd('/') + "/");
});

builder.Services.AddHttpClient<DalApiClient>((sp, http) =>
{
    var urls = sp.GetRequiredService<IOptions<ServiceUrls>>().Value;
    http.BaseAddress = new Uri(urls.DalBaseUrl.TrimEnd('/') + "/");
});

// Camping client: zit onder monolith host (HotelBaseUrl)
builder.Services.AddHttpClient<CampingApiClient>((sp, http) =>
{
    var urls = sp.GetRequiredService<IOptions<ServiceUrls>>().Value;
    http.BaseAddress = new Uri(urls.HotelBaseUrl.TrimEnd('/') + "/");
});

// Optional: YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Forwarded headers vroeg in pipeline (maakt lokaal niet stuk, maar ok)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost
});

app.UseCors();

// Optional static frontend (als je wwwroot/index.html hebt)
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.MapGet("/health", () => Results.Ok("OK"))
   .WithName("Health")
   .WithTags("System")
   .Produces<string>(StatusCodes.Status200OK);

// ===== Catalog endpoints (live uit 3 APIs) =====

app.MapGet("/api/catalog", async (
    SourceSystem? source,
    IEnumerable<ICatalogProvider> providers,
    CancellationToken ct) =>
{
    var list = new List<AccommodationCard>();

    foreach (var p in providers)
    {
        if (source is not null && p.Source != source.Value) continue;
        list.AddRange(await p.GetCatalog(ct));
    }

    return Results.Ok(list);
})
.WithName("GetCatalog")
.WithTags("Catalog")
.Produces<List<AccommodationCard>>(StatusCodes.Status200OK);

// ===== Platform endpoints =====

// 1) Availability: live catalog minus overlaps in reservation_db
app.MapGet("/api/availability", async (
    DateTime start,
    DateTime end,
    int? guests,
    int? capacityMin,
    int? capacityMax,
    SourceSystem? source,
    PlatformOrchestrator orchestrator,
    CancellationToken ct) =>
{
    try
    {
        var q = new AvailabilityQuery(start, end, guests, capacityMin, capacityMax, source);
        var cards = await orchestrator.GetAvailability(q, ct);
        return Results.Ok(cards);
    }
    catch (ReservationValidationException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("GetAvailability")
.WithTags("Availability")
.Produces<List<AccommodationCard>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

// 2) Create reservation: alleen reservation_db
app.MapPost("/api/reservations", async (
    CreateReservationRequest req,
    PlatformOrchestrator orchestrator,
    CancellationToken ct) =>
{
    try
    {
        var created = await orchestrator.CreateReservation(req, ct);
        return Results.Created($"/api/reservations/{created.PlatformReservationId}", created);
    }
    catch (ReservationConflictException ex)
    {
        return Results.Conflict(new { message = ex.Message });
    }
    catch (ReservationValidationException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("CreateReservation")
.WithTags("Reservations")
.Accepts<CreateReservationRequest>("application/json")
.Produces<ReservationCreatedResponse>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status409Conflict);

// 3) Read reservation uit reservation_db
app.MapGet("/api/reservations/{platformId:int}", async (
    int platformId,
    PlatformOrchestrator orchestrator,
    CancellationToken ct) =>
{
    var res = await orchestrator.GetReservation(platformId, ct);
    return res is null ? Results.NotFound() : Results.Ok(res);
})
.WithName("GetReservation")
.WithTags("Reservations")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// ===== Optional: reverse proxy routes =====
app.MapReverseProxy();

app.Run();
