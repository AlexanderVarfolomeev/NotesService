using Notes.NotesService;
using Notes.Settings;

namespace Notes.Api;

public static class Bootstrapper
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services
            .AddSettings()
            .AddNotesService();
        return services;
    }
}