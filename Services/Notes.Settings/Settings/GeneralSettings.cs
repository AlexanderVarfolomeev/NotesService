using Notes.Settings.Interfaces;
using Notes.Settings.Source;

namespace Notes.Settings.Settings;

public class GeneralSettings : IGeneralSettings
{
    private readonly ISettingsSource source;

    public GeneralSettings(ISettingsSource source)
    {
        this.source = source;
    }

    public bool SwaggerVisible => source.GetAsBool("General:SwaggerVisible");
}