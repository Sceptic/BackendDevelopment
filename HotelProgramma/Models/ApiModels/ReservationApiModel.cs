namespace HotelProgramma.Models
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public int AccountId { get; set; }

        public string ReservationStatus { get; set; }
        public string PaymentStatus { get; set; }

        public int Discount { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }

        public List<ReservationClientDto> Clients { get; set; }
        public List<ReservationHotelDto> Hotels { get; set; }
        public List<ReservationGiteDto> Gites { get; set; }
    }

    public class ReservationClientDto
    {
        public int ReservationId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
    }

    public class ReservationHotelDto
    {
        public int ReservationId { get; set; }
        public int RoomNumber { get; set; }
        public int HotelroomDiscount { get; set; }
    }

    public class ReservationGiteDto
    {
        public int ReservationId { get; set; }
        public int GiteNumber { get; set; }
        public int GiteDiscount { get; set; }
    }
}
