using Duende.IdentityServer.Models;
using Notes.Common;

namespace Notes.Identity.Configuration.IS4;

public static class AppApiScopes
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
           new ApiScope(AppScopes.NotesApiScope, "Access to API - Read and post data"),
        };
}