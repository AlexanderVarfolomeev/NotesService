using Microsoft.Extensions.DependencyInjection;

namespace Notes.ColorService;

public static class Bootstrapper
{
    public static IServiceCollection AddColorService(this IServiceCollection services)
    {
        services.AddSingleton<IColorService,ColorService>();
        return services;
    }
}