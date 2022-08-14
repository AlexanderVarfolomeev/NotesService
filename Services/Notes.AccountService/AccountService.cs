using Notes.AccountService.Models;
using Microsoft.AspNetCore.Identity;
using Notes.Common.Exceptions;
using Notes.EmailService;
using Notes.EmailService.Models;

namespace Notes.AccountService;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser<Guid>> userManager;
    private readonly SignInManager<IdentityUser<Guid>> sgInManager;
    private readonly IEmailSender emailSender;
    public AccountService(UserManager<IdentityUser<Guid>> userManager, SignInManager<IdentityUser<Guid>> sgInManager, IEmailSender emailSender)
    {
        this.userManager = userManager;
        this.sgInManager = sgInManager;
        this.emailSender = emailSender;
    }


    public async Task<bool> RegisterAccount(RegisterAccountModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        ProcessException.ThrowIf(() => user != null, "The user with this Email already exists.");

        user = new IdentityUser<Guid>()
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true,
            PhoneNumber = null,
            PhoneNumberConfirmed = false
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await emailSender.SendEmailAsync(new EmailModel()
            {
                Email = model.Email,
                Subject = "NotesService",
                Message = "Your account was  successfully registered!"
            });
        }
        return result.Succeeded;
    }

    public async Task<bool> DeleteAccount(AccountModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        ProcessException.ThrowIf(() => user is null, "The user with this Email was not found in the database.");

        var result = sgInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (result.Result.Succeeded)
        {
            await userManager.DeleteAsync(user);
            await emailSender.SendEmailAsync(new EmailModel()
            {
                Email = model.Email,
                Subject = "NotesService",
                Message = "Your account was  successfully deleted!"
            });
        }

        return result.Result.Succeeded;
    }
   

}

