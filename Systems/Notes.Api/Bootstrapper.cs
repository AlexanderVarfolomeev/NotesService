using Notes.AccountService;
using Notes.ColorService;
using Notes.EmailService;
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
            .AddColorService()
            .AddNotesService()
            .AddAccountService()
            .AddEmailSender();
        return services;
    }
}