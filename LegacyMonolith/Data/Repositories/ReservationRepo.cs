using LegacyMonolith.Models;
using Microsoft.EntityFrameworkCore;
using LegacyMonolith.Data.Repositories;
using LegacyMonolith.Data;

//Repositories voegen entiteiten en hun subentiteiten bijelkaar en halen deze systematisch op via de ORM. LINQ-querries worden aan de ORM gegeven, de ORM vertaald deze naar SQL-querries en haalt de bijbehorende data op.
//Een paar methodes worden gedefinieerd in elke repo om systematisch op te halen, te posten, deleten, updaten, etc. De input en output worden gevuld met de DbContext modellen die relaties beschrijven.

namespace LegacyMonolith.Application
{
    internal class ReservationRepo : IReservationRepo
    {
        private readonly DbMarconnes _db;

        public ReservationRepo(DbMarconnes db)
        {
            _db = db;
        }

        public Reservation Get(int id)
        {
            return _db.tblReservation
                .Include(x => x.ReservationHotel)
                .Include(x => x.ReservationGite)
                .Include(x => x.ReservationClient)
                .FirstOrDefault(x => x.ReservationId == id);
        }

        public List<Reservation> RetrieveReservationsByAccount(int accountId) //Retrieves all reservations that a client owns.
        {
            return _db.tblReservation
                .Include(x => x.ReservationHotel)
                .Include(x => x.ReservationGite)
                .Include(x => x.ReservationClient)
                .Where(x => x.AccountId == accountId)
                .ToList();
        }

        public bool ReservationOverlapHotel //Function investigates whether a room is already reserved, uses a daterange.
            (
            int roomNumber,
            DateTime startDate,
            DateTime endDate
            )
        {
            return _db.tblReservation
                .AsNoTracking()
                .Any
                (
                    r =>
                    r.ReservationHotel.Any
                    (
                        rh =>
                            rh.RoomNumber == roomNumber &&
                            startDate < r.ReservationEnd &&
                            endDate > r.ReservationStart
                    )
                );
        }

        public bool ReservationOverlapGite //Function investigates whether a gite is already reserved, uses a daterange.
            (
            int giteNumber,
            DateTime startDate,
            DateTime endDate
            )
        {
            return _db.tblReservation
                .AsNoTracking()
                .Any
                (
                    r =>
                    r.ReservationGite.Any
                    (
                        rh =>
                            rh.GiteNumber == giteNumber &&
                            startDate < r.ReservationEnd &&
                            endDate > r.ReservationStart
                    )
                );
        }

        // ===== ===== Write Methods, ensure EF-core tracks changes here.

        public void Post(Reservation reservation)
        {
            _db.tblReservation.Add(reservation);
        }
             
        public void Update(Reservation updated)
        {
            var existing = _db.tblReservation
                .Include(x => x.ReservationClient)
                .Include(x => x.ReservationHotel)
                .Include(x => x.ReservationGite)
                .First(x => x.ReservationId == updated.ReservationId);

            existing.Discount           = updated.Discount;
            existing.ReservationStatus  = updated.ReservationStatus;
            existing.PaymentStatus      = updated.PaymentStatus;
        }

        public void Delete(int id)
        {
            var reservation = new Reservation { ReservationId = id};
            _db.tblReservation.Attach(reservation);
            _db.tblReservation.Remove(reservation);
        }
    }

    public interface IReservationRepo
    {
        Reservation Get(int id);
        List<Reservation> RetrieveReservationsByAccount(int accountId);
        bool ReservationOverlapHotel(int hotelNumber, DateTime startDate, DateTime endDate);
        bool ReservationOverlapGite(int giteNumber, DateTime startDate, DateTime endDate);
        void Post(Reservation reservation);
        void Update(Reservation reservation);
        void Delete(int id);
    }
}
