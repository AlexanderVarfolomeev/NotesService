using Duende.IdentityServer.Models;

namespace Notes.Common
{
    public class AppScopes
    {
        public const string NotesApiScope = "notes_api";
    }

    public static class IdentityConfig
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("Notes", "Access to API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "swagger",
                    AllowedGrantTypes = new List<string>(){ GrantType.ClientCredentials},
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"Notes"}
                }   
            };
    }

}
