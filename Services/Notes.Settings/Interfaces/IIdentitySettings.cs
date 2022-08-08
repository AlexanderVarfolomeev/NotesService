namespace Notes.Settings.Interfaces
{
    public interface IIdentitySettings
    {
        string Url { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        bool RequireHttps { get; }
    }
}
