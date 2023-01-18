using Duende.IdentityServer.Models;

namespace CarSharingApp.IdentityServer
{
    public static class Config
    {
        public const string Admin = "admin";
        public const string Customer = "customer";

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId()
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope(name: "accessShareNewVehiclePage", displayName: "Share your vehicle.")
        };
    }
}
