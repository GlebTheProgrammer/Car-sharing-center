using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarSharingApp.Infrastructure.AzureKeyVault
{
    public static class Extensions
    {
        public static IServiceCollection AddAzureKeyVaultAppsettingsValues<T>(this IServiceCollection services, T configuration) where T 
            : IConfigurationBuilder, IConfigurationRoot, IDisposable
        {
            string kvURL = configuration["KeyVaultConfig:KVUrl"] 
                                    ?? throw new ArgumentNullException("KVUrl");
            string tenantId = configuration["KeyVaultConfig:TenantId"] 
                                    ?? throw new ArgumentNullException("TenantId");
            string clientId = configuration["KeyVaultConfig:ClientId"] 
                                    ?? throw new ArgumentNullException("ClientId");
            string clientSecretId = configuration["KeyVaultConfig:ClientSecretId"]
                                    ?? throw new ArgumentNullException("ClientSecretId");

            var credentials = new ClientSecretCredential(tenantId, clientId, clientSecretId);

            var client = new SecretClient(new Uri(kvURL), credentials);

            configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());

            return services;
        }
    }
}
