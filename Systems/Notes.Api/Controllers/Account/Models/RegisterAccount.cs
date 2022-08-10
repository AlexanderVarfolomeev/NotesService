using AutoMapper;
using FluentValidation;
using Notes.AccountService.Models;

namespace Notes.Api.Controllers.Account.Models;

public class RegisterAccount
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterAccountProfile : Profile
{
    public RegisterAccountProfile()
    {
        CreateMap<RegisterAccount, RegisterAccountModel>();
    }
}

public class RegisterAccountValidator : AbstractValidator<RegisterAccount>
{
    public RegisterAccountValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).MinimumLength(6).WithMessage("Минимальная допустимая длина пароля - 6 символов!");
        RuleFor(x => x.Password).MaximumLength(16).WithMessage("Максимальная допустимая длина пароля - 16 символов!");
    }
}
