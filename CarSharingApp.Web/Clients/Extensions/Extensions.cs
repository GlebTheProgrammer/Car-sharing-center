using Azure.Storage.Blobs;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Clients.Policies;

namespace CarSharingApp.Web.Clients.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterNewHttpClients(this IServiceCollection services, string identifier, IConfiguration configuration)
        {
            services.AddHttpClient(configuration[$"Clients:{identifier}:Name"]
                ?? throw new ArgumentNullException(identifier),
            client =>
            {
                client.BaseAddress = new Uri(configuration[$"Clients:{identifier}:BaseAddress"]
                    ?? throw new ArgumentNullException(identifier));
            }).AddPolicyHandler(
                request => request.Method == HttpMethod.Get ? new ClientPolicy(3).BackoffHttpRetry 
                                                            : new ClientPolicy(3).BackoffHttpRetry);

            return services;
        }

        public static IServiceCollection RegisterAzureBlobStorageClient<T>(this IServiceCollection services, T configuration) 
            where T : IConfigurationBuilder, IConfigurationRoot, IDisposable
        {
            services.AddSingleton(s => new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]));

            services.AddSingleton<IAzureBlobStoragePublicApiClient, BlobStoragePublicApiClient>();

            return services;
        }
    }
}
