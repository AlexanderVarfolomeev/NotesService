namespace Notes.Settings.Interfaces;

/// <summary>
/// Настройки связанные с API
/// </summary>
public interface IApiSettings
{
    IDbSettings Db { get; }
    IGeneralSettings General { get; }
}