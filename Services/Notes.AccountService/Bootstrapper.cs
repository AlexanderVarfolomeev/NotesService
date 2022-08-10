using Microsoft.Extensions.DependencyInjection;

namespace Notes.AccountService;

public static class Bootstrapper
{
    public static IServiceCollection AddAccountService(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}