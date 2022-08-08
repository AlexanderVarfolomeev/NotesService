using Notes.Settings.Interfaces;
using Notes.Settings.Source;

namespace Notes.Settings.Settings
{
    public class IS4Settings : IIS4Settings
    {
        private readonly ISettingsSource source;
        private readonly IDbSettings dbSettings;

        public IS4Settings(ISettingsSource source) => this.source = source;

        public IS4Settings(ISettingsSource source, IDbSettings dbSettings)
        {
            this.source = source;
            this.dbSettings = dbSettings;
        }

        public IDbSettings Db => dbSettings ?? new DbSettings(source);
    }

}
