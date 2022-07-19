using Notes.Api;
using Notes.Api.Configuration;
using Notes.Settings.Settings;
using Notes.Settings.Source;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var settings = new ApiSettings(new SettingsSource());

// read configuration for serilog
builder.Host.UseSerilog((host, cfg) =>
{
    cfg.ReadFrom.Configuration(host.Configuration);
});

services.AddAppDbContext(settings);

services.AddControllers();

services.AddAppSwagger();

services.AddAppServices();

services.AddAutoMappers();

var app = builder.Build();

app.UseAuthorization();

app.UseRouting();

app.UseSerilogRequestLogging();

app.MapControllers();

app.UseAppSwagger();

app.UseAppDbContext();

app.Run();
