using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Notes.WPF.Models.Auth;
using Notes.WPF.Models.Notes;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.Validators.Account
{
    internal static class AccountValidator
    {
        private static readonly IUserDialogService userDialogService;

        static AccountValidator()
        {
            userDialogService = new UserDialogService();
        }

        public static bool Check(LoginModel account)
        {
            if (account.Password.Length < 6)
            {
                userDialogService.ShowError("Пароль должен быть больше 6 символов!", "Ошибка регистрации.");
                return false;
            }

            var regexEmail = @"(@)(.+)$";

            if (!Regex.IsMatch(account.Email, regexEmail))
            {
                userDialogService.ShowError("Введите почту в корректном формате!", "Ошибка регистрации.");
                return false;
            }

            return true;
        }
    }
}