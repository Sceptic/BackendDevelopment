using Application;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

//Definieert een geconfigureerde manier om errors te handelen.
app.UseExceptionHandler(errorApp =>
{
    //Als een error gegooid wordt binnen de applicatie wordt in dit blok ingesprongen
    errorApp.Run(async context =>
    {
        //De applicatie haalt de concrete error op
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        //Mapt de verschillende C# runtime errors naar verschillende HTTP-error codes
        var status = ex switch
        {
            //Indien de invariants een waarde hebben gevonden die niet logisch of relationeel correct is, wordt deze mapping gebruikt
            Domain.ErrorHandling.DomainValidationException => StatusCodes.Status422UnprocessableEntity,

            //Indien de database een error gooit (specifiek in het geval giteNumber die een duplicaat is)
            DbUpdateException dbEx when IsUniqueViolation(dbEx) => StatusCodes.Status409Conflict,

            //Velden die met een foute data type worden ingevuld (stringvelden die bijvoorbeeld met een integer gevoerd worden)
            ArgumentException => StatusCodes.Status400BadRequest,

            //Alle overige errors, er wordt hierbij vanuit gegaan dat het de schuld van de server is
            _ => StatusCodes.Status500InternalServerError
        };

        //Geeft korte, niet-descriptieve beschrijvingen voor eindgebruikers, is expres waag voor een prod omgeving
        var title = status switch
        {
            StatusCodes.Status409Conflict => "Conflict",
            StatusCodes.Status422UnprocessableEntity => "Validation failed",
            StatusCodes.Status400BadRequest => "Bad request",
            _ => "Server error"
        };

        //Vormt één groot error message voor de gebruiker
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = app.Environment.IsDevelopment() ? ex?.Message : null //Uitgebreide error messages ALLEEN in development mode
        };

        if (ex is Domain.ErrorHandling.DomainValidationException dv) //Als het een invariant error is, dan ...
            problem.Extensions["errors"] = dv.Errors; //Voeg de error zoals die direct in de dictionary is beschreven

        if (status == StatusCodes.Status409Conflict) //Als het probleem vanuit de database komt ...
            problem.Extensions["errors"] = new Dictionary<string, string[]> //Maak een nieuwe dictionary
            {
                ["GiteNumber"] = new[] { "A gite with this number already exists." } //Zet een key "GiteNumber" en geef een korte/wage beschrijving als de value
            };

        //Stuurt de daadwerkelijke error message richting de gebruiker
        context.Response.StatusCode = status; //Geeft de http error code.
        context.Response.ContentType = "application/problem+json"; //Voegt de body toe
        await context.Response.WriteAsJsonAsync(problem);
    });
});

static bool IsUniqueViolation(DbUpdateException ex)
{
    if (ex.InnerException is SqlException sql)
        return sql.Number is 2601 or 2627; //Sql errors 2601 en 2627 zijn duplicaten errors

    return false;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
