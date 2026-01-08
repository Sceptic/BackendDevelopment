using LegacyMonolith.Models;
using Microsoft.EntityFrameworkCore;
using LegacyMonolith.Data;

//Repositories voegen entiteiten en hun subentiteiten bijelkaar en halen deze systematisch op via de ORM. LINQ-querries worden aan de ORM gegeven, de ORM vertaald deze naar SQL-querries en haalt de bijbehorende data op.
//Een paar methodes worden gedefinieerd in elke repo om systematisch op te halen, te posten, deleten, updaten, etc. De input en output worden gevuld met de DbContext modellen die relaties beschrijven.

namespace LegacyMonolith.Data.Repositories
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
