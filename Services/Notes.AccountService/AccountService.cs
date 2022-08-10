using Notes.AccountService.Models;
using Microsoft.AspNetCore.Identity;
namespace Notes.AccountService;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser<Guid>> userManager;
    private readonly SignInManager<IdentityUser<Guid>> sgInManager;

    public AccountService(UserManager<IdentityUser<Guid>> userManager, SignInManager<IdentityUser<Guid>> sgInManager)
    {
        this.userManager = userManager;
        this.sgInManager = sgInManager;
    }


    public async Task<bool> RegisterAccount(RegisterAccountModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
            throw new Exception(""); // исправить на новое исключение

        user = new IdentityUser<Guid>()
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true,
            PhoneNumber = null,
            PhoneNumberConfirmed = false
        };


        var result = await userManager.CreateAsync(user, model.Password);
        return result.Succeeded;
    }

    public async Task<bool> DeleteAccount(AccountModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
            throw new NotImplementedException();

        var result = sgInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (result.Result.Succeeded)
        {
            await userManager.DeleteAsync(user);
        }

        return result.Result.Succeeded;
    }
   

}

