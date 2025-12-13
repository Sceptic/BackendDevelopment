using HotelProgramma.Models;
using Microsoft.EntityFrameworkCore;
using HotelProgramma.Data.Repositories;

namespace HotelProgramma.Data.Repositories
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
        void Post(Reservation reservation);
        void Update(Reservation reservation);
        void Delete(int id);
    }
}
