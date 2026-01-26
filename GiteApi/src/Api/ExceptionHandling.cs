using Domain.ErrorHandling;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GlobalErrorHandling
{
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    var status = ex switch
                    {
                        DomainValidationException => StatusCodes.Status422UnprocessableEntity,
                        DbUpdateException dbEx when DbUpdateExceptionExtensions.IsUniqueViolation(dbEx) => StatusCodes.Status409Conflict,
                        ArgumentException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    var title = status switch
                    {
                        StatusCodes.Status409Conflict => "Conflict",
                        StatusCodes.Status422UnprocessableEntity => "Validation failed",
                        StatusCodes.Status400BadRequest => "Bad request",
                        _ => "Server error"
                    };

                    var problem = new ProblemDetails
                    {
                        Status = status,
                        Title = title,
                        Detail = app is WebApplication webApp && webApp.Environment.IsDevelopment()
                            ? ex?.Message
                            : null
                    };

                    if (ex is DomainValidationException dv)
                        problem.Extensions["errors"] = dv.Errors;

                    if (status == StatusCodes.Status409Conflict)
                        problem.Extensions["errors"] = new Dictionary<string, string[]>
                        {
                            ["GiteNumber"] = new[] { "A gite with this number already exists." }
                        };

                    context.Response.StatusCode = status;
                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(problem);
                });
            });

            return app;
        }
    }

    public static class DbUpdateExceptionExtensions
    {
        public static bool IsUniqueViolation(this DbUpdateException ex)
        {
            return ex.InnerException is SqlException sql
                && (sql.Number == 2601 || sql.Number == 2627);
        }
    }
}


