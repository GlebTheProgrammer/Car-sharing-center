using Duende.IdentityServer.Models;

namespace CarSharingApp.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId()
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope(name: "CarSharingService.IdentityServer.API", displayName: "DuendeIdentityServerAPI")
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "CarSharingService.IdentityServer.API" }
            }
        };
    }
}
