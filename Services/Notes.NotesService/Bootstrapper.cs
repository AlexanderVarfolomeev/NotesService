using Microsoft.Extensions.DependencyInjection;

namespace Notes.NotesService;

public static class Bootstrapper
{
    public static IServiceCollection AddNotesService(this IServiceCollection services)
    {
        services.AddSingleton<INotesService, NotesService>();
        return services;
    }
}