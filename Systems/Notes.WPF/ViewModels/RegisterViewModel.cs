using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notes.WPF.Models.Auth;
using Notes.WPF.Services.Auth;
using Notes.WPF.Services.Notes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.Services.UserDialog;
using Notes.WPF.Validators.Account;

namespace Notes.WPF.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IUserDialogService _userDialogService;
    private readonly IAuthService _authService;

    public RegisterViewModel(IUserDialogService userDialogService, IAuthService authService)
    {
        _userDialogService = userDialogService;
        _authService = authService;

        RegisterLogin = "user@mail.ru";
        RegisterPassword = "pass123";
    }
    [ObservableProperty] private string _registerLogin;
    [ObservableProperty] private string _registerPassword;

    [RelayCommand]
    public async Task CloseAndSave(object parameter)
    {
        if (await Save())
        {
            var window = (Window)parameter;
            window.Close();
        }
    }

    private async Task<bool> Save()
    {
        var model = new LoginModel()
        {
            Email = RegisterLogin,
            Password = RegisterPassword
        };
        if (!AccountValidator.Check(model))
            return false;


        return await _authService.Register(model);
    }

}