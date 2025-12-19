using System;
using System.Collections.Generic;
using System.Text;

//De ORM gebruikt deze klasses (alle klasses binnen de DbModels directory) om data op te halen en op te sturen naar de database.
//De DbModels bevatten een volledige beschrijving van relaties, waarbij er een directe koppeling gemaakt wordt tussen andere gerelateerde objecten om een "graph" (kaart) te maken.
//De reservationentiteit is een goed voorbeeld hiervan, het heeft een een-op-een relatie met Account

//Ga eens naar de Account datatype en lees de commentaar.

namespace HotelProgramma.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; } //Een-op-een relatie, bevat één account voor elke reservatie (control left-click eens op de account datatype, de tekst direct na "public").
                                             //Het ding is, je kan nu eindeloos tussen de twee heen gaan (control left-clicken), dit is ook precies wat de .json
                                             //serializer gaat doen als je het de DbModels geeft, het gaat dan eindeloos heen-en-weer totdat het crasht,
                                             //Hierbij heb je dus de ApiModels (Dto's) voor nodig.

        public string ReservationStatus { get; set; }
        public string PaymentStatus { get; set; }

        public int Discount { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }

        public ICollection<ReservationClient> ReservationClient { get; set; }
        public ICollection<ReservationGite> ReservationGite { get; set; }
        public ICollection<ReservationHotel> ReservationHotel { get; set; }
    }
}
