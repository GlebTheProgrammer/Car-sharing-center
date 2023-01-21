using CarSharingApp.Domain.Entities;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CarSharingApp.IdentityServer.StaticFiles
{
    public static class IdentityServerConfigurations
    {
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets =
                    {
                        new Secret("client_secret_mvc".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.Code,

                    AllowedScopes =
                    {
                        "PublicAPI.CustomerEndpoints",

                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },

                    RedirectUris =
                    {
                        "https://localhost:44362/signin-oidc"
                    }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope(name: "PublicAPI.CustomerEndpoints", displayName: "Get access to customer endpoints")
            };

        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };





        public static void CreateNewIdentityUser(IServiceProvider scopeServiceProvider, Customer customer)
        {
            var userManager = scopeServiceProvider.GetService<UserManager<IdentityUser>>();

            var user = new IdentityUser
            {
                Id = customer.Id.ToString(),
                Email = customer.Credentials.Email,
                PhoneNumber = customer.PhoneNumber,
                UserName = customer.Credentials.Login,
                PasswordHash = customer.Credentials.Password
            };

            var result = userManager?.CreateAsync(user, customer.Credentials.Password).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                userManager?.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Customer")).GetAwaiter().GetResult();
            }
        }


    }
}
