using HotelProgramma.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using HotelProgramma.Data;

namespace HotelProgramma.Application
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
