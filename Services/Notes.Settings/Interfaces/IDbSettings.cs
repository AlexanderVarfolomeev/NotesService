namespace Notes.Settings.Interfaces;

public interface IDbSettings
{
    string GetConnectionString { get; }
}