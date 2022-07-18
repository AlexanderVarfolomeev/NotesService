using Microsoft.Extensions.DependencyInjection;

namespace Notes.Common.Helpers
{
    public static class AutoMapperRegisterServices
    {
        public static void Register(IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(s => s.FullName != null && s.FullName.ToLower().StartsWith("notes."));

            services.AddAutoMapper(assemblies);
        }
    }
}
