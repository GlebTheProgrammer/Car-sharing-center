using Polly;
using Polly.Contrib.WaitAndRetry;

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
            })
            .AddTransientHttpErrorPolicy(
            polictBuilder =>
                polictBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 4)));

            return services;
        }
    }
}
