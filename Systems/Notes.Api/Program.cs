using Notes.Api.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// read configuration for serilog
builder.Host.UseSerilog((host, cfg) =>
{
    cfg.ReadFrom.Configuration(host.Configuration);
});


services.AddControllers();

services.AddAppSwagger();



var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.UseAppSwagger();

app.Run();
