namespace Notes.Settings.Interfaces;

public interface IApiSettings
{
    IDbSettings Db { get; }
    IGeneralSettings General { get; }
    IIdentitySettings IdentityServer { get; }
}