using Notes.Context.Context;
using Notes.Context.Factories;
using Notes.Context.Setup;
using Notes.Settings.Interfaces;

namespace Notes.Api.Configuration;

public static class DbConfiguration
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IApiSettings settings)
    {
        var dbOptDelegate = DbContextOptionsFactory.Configure(settings.Db.GetConnectionString);
        services.AddDbContextFactory<MainDbContext>(dbOptDelegate, ServiceLifetime.Singleton);
        return services;
    }

    public static WebApplication UseAppDbContext(this WebApplication app)
    {
        DbInit.Execute(app.Services);
        DbSeed.Execute(app.Services);
        return app;
    }
}