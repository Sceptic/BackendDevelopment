using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

//Ga naar Reservation.cs voor uitleg over de DbModels.

namespace HotelProgramma.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public ICollection<Reservation> Reservations { get; set; } //Hier zie je de andere kant van de relatie, een account kan meerdere reservaties hebben.
                                                                   //De lijst van reservaties worden niet direct in een lijst gezet, maar worden door middel van een ICollection "lijst" bijgehouden.
                                                                   //Deze lijst wordt door C# zelf niet gebruikt maar wordt wél door de ORM gevuld om bij te houden met welke objecten één object relaties heeft.
                                                                   //Klik op de datatype <Reservation> om terug te gaan en lees de commentaar naast Account even door.
    }
}
