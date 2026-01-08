using LegacyMonolith.Data;
using LegacyMonolith.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegacyMonolith.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public AccountController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("{accountId}")]
        public ActionResult<AccountDto> Get(int accountId)
        {
            var account = _uow.Accounts.Get(accountId);

            if (account == null)
                return NotFound();

            var dto = new AccountDto
            {
                AccountId = account.AccountId,
            };

            return Ok(dto);
        }
    }
}
