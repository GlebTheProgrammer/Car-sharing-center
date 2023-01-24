using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace CarSharingApp.Infrastructure.AzureAD
{
    public static class Extensions
    {
        public static IServiceCollection ConfigureAzureAD<T>(this IServiceCollection services, T configuration) where T
            : IConfigurationBuilder, IConfigurationRoot, IDisposable
        {
            services.AddMicrosoftIdentityWebApiAuthentication(configuration);

            return services;
        }
    }
}
