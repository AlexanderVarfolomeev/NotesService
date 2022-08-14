using Microsoft.AspNetCore.Mvc;

namespace Notes.Api.Configuration;

public static class VersioningConfiguration
{
    public static IServiceCollection AddAppVersions(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
        });

        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

}