using Duende.IdentityServer.Models;

namespace Notes.Identity.Configuration.IS4;

public static class AppResources
{
    public static IEnumerable<ApiResource> Resources => new List<ApiResource>
    {
        new ApiResource("api")
    };
}
