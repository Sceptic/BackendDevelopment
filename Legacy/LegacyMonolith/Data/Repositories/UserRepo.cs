using LegacyMonolith.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using LegacyMonolith.Data;

//Repositories voegen entiteiten en hun subentiteiten bijelkaar en halen deze systematisch op via de ORM. LINQ-querries worden aan de ORM gegeven, de ORM vertaald deze naar SQL-querries en haalt de bijbehorende data op.
//Een paar methodes worden gedefinieerd in elke repo om systematisch op te halen, te posten, deleten, updaten, etc. De input en output worden gevuld met de DbContext modellen die relaties beschrijven.

namespace LegacyMonolith.Application
{
    internal class UserRepo : IUserRepo
    {
        private readonly DbMarconnes _db;

        public UserRepo(DbMarconnes db)
        {
            _db = db;
        }

        public Account Get(int id)
        {
            return _db.tblAccount
                .FirstOrDefault(x => x.AccountId == id);
        }

        // ===== ===== Write Methods, ensure EF-core tracks changes here.

        public void Post(Account account)
        {
            _db.tblAccount.Add(account);
        }

        public void Delete(int id)
        {
            var account = new Account { AccountId = id};
            _db.tblAccount.Attach(account);
            _db.tblAccount.Remove(account);
        }
    }

    public interface IUserRepo
    {
        Account Get(int id);
        void Post(Account account);
        void Delete(int id);
    }
}
