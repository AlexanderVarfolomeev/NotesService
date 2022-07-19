using Notes.NotesService;
using Notes.Settings;
using Notes.TaskTypeService;

namespace Notes.Api;

public static class Bootstrapper
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services
            .AddSettings()
            .AddTaskTypesService()
            .AddNotesService();
        return services;
    }
}