using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notes.WPF.Models.Auth;
using Notes.WPF.Services.Auth;
using Notes.WPF.Services.Colors.Models;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IUserDialogService _userDialogService;
    private readonly IAuthService _authService;

    public LoginViewModel(IUserDialogService userDialogService, IAuthService authService)
    {
        _userDialogService = userDialogService;
        _authService = authService;

        Email = "user@mail.ru";
        Password = "pass123";
    }

    [ObservableProperty] private string _email;
    [ObservableProperty] private string _password;

    [RelayCommand]
    private async Task Login()
    {
        LoginModel loginModel = new LoginModel()
        {
            Email = Email,
            Password = Password
        };
        bool successful = (await _authService.Login(loginModel)).Successful;
        if (!successful)
            _userDialogService.ShowError("Неверные логин или пароль!", "Ошибка идентификации.");
        else
        {
            await _userDialogService.OpenMainWindow();
        }
    }

    [RelayCommand]
    private void Register()
    {
        _userDialogService.OpenRegisterWindow();
    }
}