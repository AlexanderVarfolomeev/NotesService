using Microsoft.Extensions.DependencyInjection;

namespace Notes.EmailService;

public static class Bootstrapper
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, EmailSender>();

        return services;
    }
}