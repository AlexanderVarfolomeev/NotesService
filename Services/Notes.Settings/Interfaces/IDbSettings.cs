namespace Notes.Settings.Interfaces;

/// <summary>
/// Настройки связанные с бд
/// </summary>
public interface IDbSettings
{
    string GetConnectionString { get; }
}