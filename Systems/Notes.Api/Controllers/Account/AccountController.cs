using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes.AccountService;
using Notes.AccountService.Models;
using Notes.Api.Controllers.Account.Models;

namespace Notes.Api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            this.accountService = accountService;
            this.mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task Register([FromBody] Models.Account model)
        {
            var user = mapper.Map<AccountModel>(model);
            await accountService.RegisterAccount(user);
        }

        [HttpDelete("Delete")]
        public async Task Delete([FromBody] Models.Account model)
        {
            var user = mapper.Map<AccountModel>(model);
            await accountService.DeleteAccount(user);
        }

    }
}
