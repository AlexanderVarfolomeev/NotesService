using Microsoft.Extensions.DependencyInjection;

namespace Notes.WPF.ViewModels;

public static class LocatorStatic
{
    public static MainWindowViewModel mainVM = App.Host.Services.GetRequiredService<MainWindowViewModel>();
    public static RegisterViewModel registerVM = App.Host.Services.GetRequiredService<RegisterViewModel>();
}