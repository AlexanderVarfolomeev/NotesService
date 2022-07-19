using Microsoft.Extensions.DependencyInjection;

namespace Notes.TaskTypeService;

public static class Bootstrapper
{
    public static IServiceCollection AddTaskTypesService(this IServiceCollection services)
    {
        services.AddSingleton<ITaskTypeService, TaskTypeService>();
        return services;
    }
}