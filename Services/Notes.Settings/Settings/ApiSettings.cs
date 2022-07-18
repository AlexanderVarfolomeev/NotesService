using Notes.Settings.Interfaces;
using Notes.Settings.Source;

namespace Notes.Settings.Settings;

public class ApiSettings :IApiSettings
{
    private readonly ISettingsSource source;
    private readonly IGeneralSettings generalSettings;
    private readonly IDbSettings dbSettings;

    public ApiSettings(ISettingsSource source)
    {
        this.source = source;
    }

    public ApiSettings(ISettingsSource source, IGeneralSettings generalSettings, IDbSettings dbSettings)
    {
        this.source = source;
        this.generalSettings = generalSettings;
        this.dbSettings = dbSettings;
    }
    public IGeneralSettings General => generalSettings ?? new GeneralSettings(source);

    public IDbSettings Db => dbSettings ?? new DbSettings(source);
}