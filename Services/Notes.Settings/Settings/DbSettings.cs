using Notes.Settings.Interfaces;
using Notes.Settings.Source;

namespace Notes.Settings.Settings;

public class DbSettings : IDbSettings
{
    private readonly ISettingsSource source;

    public DbSettings(ISettingsSource source)
    {
        this.source = source;
    }
    public string GetConnectionString => source.GetConnectionString("MainConnectionString");
}