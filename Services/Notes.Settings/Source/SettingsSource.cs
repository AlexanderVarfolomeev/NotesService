using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Notes.Settings.Source;

public class SettingsSource : ISettingsSource
{
    private readonly IConfiguration? configuration;

    public SettingsSource(IConfiguration? conf = null)
    {

        if (conf != null)
        {
            configuration = conf;
            return;
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json", optional: false);

        var aspNetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
        bool idDev = aspNetCoreEnv.ToLower().Equals("development");

        if (idDev)
            builder.AddJsonFile("appsettings.development.json", optional: true);

        configuration = builder
            .AddEnvironmentVariables()
            .Build();
    }

    public string GetConnectionString(string source = null)
    {
        return ApplyEnvironmentVariable(configuration.GetConnectionString(source ?? "ConnectionString"));
    }

    public string GetAsString(string source, string defaultValue = null)
    {
        var val = ApplyEnvironmentVariable(configuration[source]);
        return val ?? defaultValue;
    }

    public bool GetAsBool(string source, bool defaultValue = false)
    {
        var val = ApplyEnvironmentVariable(configuration[source]);
        return bool.TryParse(val, out var result) ? result : defaultValue;
    }

    public int GetAsInt(string source, int defaultValue = 0)
    {
        var val = ApplyEnvironmentVariable(configuration[source]);
        return int.TryParse(val, out var result) ? result : defaultValue;
    }

    private string ApplyEnvironmentVariable(string value)
    {
        value ??= "";
        var list = Regex.Matches(value, @"{[^}]+}").OfType<Match>().Select(c => c.Value).ToList();

        foreach (var itm in list)
        {
            var v = itm.Replace("{", "").Replace("}", "");
            value = value.Replace($"{{{v}}}", configuration[$"{v}"]);
        }

        return value;
    }
}