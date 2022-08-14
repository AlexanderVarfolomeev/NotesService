using Microsoft.Extensions.DependencyInjection;
using Notes.Settings.Interfaces;
using Notes.Settings.Settings;
using Notes.Settings.Source;

namespace Notes.Settings;

/// <summary>
/// Класс для добавления реализации всех интерфейсов
/// </summary>
public static class Bootstrapper
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsSource, SettingsSource>();
        services.AddSingleton<IApiSettings, ApiSettings>();
        services.AddSingleton<IDbSettings, DbSettings>();
        services.AddSingleton<IGeneralSettings, GeneralSettings>();
        services.AddSingleton<IIdentitySettings, IdentitySettings>();
        services.AddSingleton<IEmailSettings, EmailSettings>();
        return services;
    }
}