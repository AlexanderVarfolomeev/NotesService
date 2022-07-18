namespace Notes.Settings.Source;

/// <summary>
/// С помощью этого интерфейса мы можем извлекать данные из файала конфигурации.
/// </summary>
public interface ISettingsSource
{
    string GetConnectionString(string source = null);
    string GetAsString(string source, string defaultValue = null);
    bool GetAsBool(string source, bool defaultValue = false);
    int GetAsInt(string source, int defaultValue = 0);
}