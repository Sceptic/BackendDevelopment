using Application;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        //Dit is een switch expression, 
        var status = ex switch
        {
            //Checkt of er sprake is van een bekende input-error
            //Het kijkt specifiek of er een error is van de type "DomainValidationException",
            //Deze errors zijn de errors die gegooit worden door de invariants als er onlogische onzin waardes door de gebruikers gegeven wordt.
            Domain.ErrorHandling.DomainValidationException => StatusCodes.Status422UnprocessableEntity,
            
            //Checkt voor andere overige errors, zoals bijvoorbeeld: strings waar ints verwacht werden, andere overige errors vanuit
            //de gebruiker
            ArgumentException => StatusCodes.Status400BadRequest,
            
            //"_" betekent "alles anders", in andere woorden, alle andere errors zijn "Interne server errors".
            //Er wordt vanuit gegaan dat de rest de schuld is van de server en niet de gebruiker.
            _ => StatusCodes.Status500InternalServerError
        };

        //Bereid een systematische error-message voor volgens RFC-7807 standaard.
        var problem = new ProblemDetails
        {
            Status = status, //Neemt de output van de eerdere switch statement (een error code bijv. "422") en voegt het hier toe
            Title = status == 500 ? "Server error" : "Validation failed", //Vraagt: is dit een 500 error? Dan zeg "Server error", anders zeg "Validation failed"
            Detail = app.Environment.IsDevelopment() ? ex?.Message : null //Geeft deze error-message alleen in een development omgeving.
        };

        if (ex is Domain.ErrorHandling.DomainValidationException dv)
            problem.Extensions["errors"] = dv.Errors; //Zorgt ervoor dat de DomainException de dictionary met errors krijgt.

        //Dit laatste gedeelte stuurt de daadwerkelijke error-message richting de gebruiker.
        context.Response.StatusCode = status; //De packet krijgt de error-code in de header
        context.Response.ContentType = "application/problem+json"; //Vormt de payload met de ProblemDetails object
        await context.Response.WriteAsJsonAsync(problem); //Stuurt de packet
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
