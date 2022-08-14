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

services.AddHttpContextAccessor();

services.AddAppVersions();

services.AddAppDbContext(settings);

services.AddAppSwagger(settings);

services.AddAppCors();

services.AddAppServices();

services.AddAppAuth(settings);

services.AddControllers().AddValidator();

services.AddRazorPages();

services.AddAutoMappers();


var app = builder.Build();

app.UseAppMiddlewares();

app.UseStaticFiles();

app.UseAuthorization();

app.UseRouting();

app.UseAppCors();

app.UseSerilogRequestLogging();

app.UseAppAuth();

app.MapRazorPages();

app.MapControllers();

app.UseAppSwagger();

app.UseAppDbContext();

app.Run();
