using Notes.AccountService.Models;

namespace Notes.AccountService;

public interface IAccountService
{
    Task<bool> RegisterAccount(RegisterAccountModel model);
    Task<bool> DeleteAccount(AccountModel model);
}