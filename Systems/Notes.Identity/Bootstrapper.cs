using Notes.Settings;

namespace Notes.Identity
{
    public static class Bootstrapper
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddSettings();
        }
    }
}
