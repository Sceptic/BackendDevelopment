using HotelProgramma.Data;
using HotelProgramma.Data.Repositories;

namespace HotelProgramma.Data
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DbMarconnes _db;

        public IUserRepo Accounts { get; }
        public IReservationRepo Reservations { get; }
        public IGiteRepo Gites { get; }
        public IHotelroomRepo Hotels { get; }

        public UnitOfWork()
        {
            _db = new DbMarconnes();

            Accounts = new UserRepo(_db);
            Reservations = new ReservationRepo(_db);
            Gites = new GiteRepo(_db);
            Hotels = new HotelroomRepo(_db);
        }

        public int Complete()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        IUserRepo Accounts { get; }
        IReservationRepo Reservations { get; }
        IGiteRepo Gites { get; }
        IHotelroomRepo Hotels { get; }
        int Complete();
    }
}
