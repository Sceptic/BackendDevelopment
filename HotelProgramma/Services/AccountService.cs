using HotelProgramma.Data;
using HotelProgramma.Models;

namespace HotelProgramma.Services
{
    public sealed class AccountService
    {
        private readonly IUnitOfWork _uow;

        public AccountService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Account GetAccount(int id)
        {
            return _uow.Accounts.Get(id);
        }

        public void CreateAccount(Account account)
        {
            _uow.Accounts.Post(account);
            _uow.Complete();
        }

        //public void DeleteAccount(int id)
        //{
            //var account = _uow.Accounts.Get(id);
            //if (account == null) return;

            //_uow.Accounts.Delete(account);
            //_uow.Complete();
        //}
    }
}
