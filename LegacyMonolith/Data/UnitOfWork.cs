using LegacyMonolith.Application;
using LegacyMonolith.Data;
using LegacyMonolith.Data.Repositories;

//Geeft een soort van "toolkit"/gereedschapskistje aan business logica, hierin staat een lijst van interfaces en transactielogica gegeven die businesslogica mag gebruiken.
//De APIs kunnen het gebruiken om data op te halen vanuit de DAL (Data access layer/data laag), hiermee krijgen zij gecontroleerde en indirecte toegang tot data.
//Hierbij kan business logica geen eigen querries creëren of uitvoeren en wordt het geforceerd om de logica van de UOW te gebruiken. Wordt een keer opgeroepen, gebruikt en wordt dan weggegooid na gebruik,
//daarom ook de naam "Unit Of Work" ("Eenheid van werk").

namespace LegacyMonolith.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DbMarconnes _db;

        public IUserRepo Accounts { get; }
        public IReservationRepo Reservations { get; }
        public IGiteRepo Gites { get; }
        public IHotelroomRepo Hotels { get; }

        public UnitOfWork(DbMarconnes db)
        {
            _db = db;

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
