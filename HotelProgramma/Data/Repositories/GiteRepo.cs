using HotelProgramma.Models;
using Microsoft.EntityFrameworkCore;
using HotelProgramma.Data;

namespace HotelProgramma.Data.Repositories
{
    internal class GiteRepo : IGiteRepo
    {
        private readonly DbMarconnes _db;

        public GiteRepo(DbMarconnes db)
        {
            _db = db;
        }

        public Gite Get(int giteNumber)
        {
            return _db.tblGite
                .FirstOrDefault(x => x.GiteNumber == giteNumber);
        }

        // ===== ===== Write Methods, ensure EF-core tracks changes here.

        public void Post(Gite gite)
        {
            _db.tblGite.Add(gite);
        }

        public void Update(Gite updated)
        {
            var existing = _db.tblGite
                .First(x => x.GiteNumber == updated.GiteNumber);

            existing.GitePrice = updated.GitePrice ?? existing.GitePrice;
            existing.IsAvailable = updated.IsAvailable ?? existing.IsAvailable;
            existing.GiteAddress = updated.GiteAddress ?? existing.GiteAddress;
            existing.CapacityMin = updated.CapacityMin ?? existing.CapacityMin;
            existing.CapacityMax = updated.CapacityMax ?? existing.CapacityMax;
        }

        public void Delete(int giteNumber)
        {
            var gite = new Gite { GiteNumber = giteNumber };
            _db.tblGite.Attach(gite);
            _db.tblGite.Remove(gite);
        }
    }

    public interface IGiteRepo
    {
        Gite Get(int id);
        void Post(Gite gite);
        void Update(Gite updated);
        void Delete(int giteNumber);
    }
}
