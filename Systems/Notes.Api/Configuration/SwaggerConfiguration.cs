using System.Runtime.CompilerServices;
using Notes.Settings.Interfaces;

namespace Notes.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        return services;
    }

    public static WebApplication UseAppSwagger(this WebApplication app)
    {
        var settings = app.Services.GetService<IApiSettings>();
        if (!settings.General.SwaggerVisible)
            return app;
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}