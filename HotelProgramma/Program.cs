using HotelProgramma.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HotelProgramma
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Maakt een nieuwe WebApplication builder.
            // 'args' zijn de commandline-argumenten waarmee de app gestart wordt.
            var builder = WebApplication.CreateBuilder(args);

            Console.WriteLine(builder);

            // AVoegt MVC-ondersteuning toe (Controllers + Views).
            // Zonder dit kan je geen controllers en razor-views gebruiken.
            builder.Services.AddControllersWithViews();

            // Swagger Services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Registreert de DAL-klasse in de dependency injection container.
            // AddScoped betekent:
            // - Voor elke HTTP-request wordt precies één DAL-instantie gemaakt.
            // - Diezelfde instantie wordt binnen die request gedeeld.
            builder.Services.AddScoped<DAL>();

            // Bouwt de WebApplication op basis van de configuratie en geregistreerde services.
            var app = builder.Build();

            // Controleert of de app NIET in Development draait.
            // In Productie wil je geen gedetailleerde error-pagina's laten zien aan de gebruiker.
            if (app.Environment.IsDevelopment())
            {

                // Swagger alleen in Development gebruiken.
                // Als je het altijd wil, haal de if weg.
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Stuurt alle HTTP-verzoeken door naar HTTPS.
            app.UseHttpsRedirection();

            // Zorgt dat routing actief is. Nodig om URL's aan controllers/actions te koppelen.
            app.UseRouting();

            // Voegt autorisatie-middleware toe.
            // (Je gebruikt dit pas echt als je met [Authorize]-attributen werkt.)
            app.UseAuthorization();

            // Maakt statische bestanden (css, js, images) beschikbaar via de webserver.
            // // In nieuwe templates wordt dit via MapStaticAssets / WithStaticAssets gedaan.
            app.MapStaticAssets();
          
            // Standaard route-configuratie:
            // URL patroon: /{controller}/{action}/{id?}
            // - controller: standaard "Home"
            // - action: standaard "Index"
            // - id: optioneel
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets(); // combineert route met statische assets-handling.

            var ConsoleApp = new ConsoleApplicationTestingInterface();

            // Start de webapplicatie en gaat luisteren naar inkomende HTTP-verzoeken.
            app.Run();
        }
    }
}
