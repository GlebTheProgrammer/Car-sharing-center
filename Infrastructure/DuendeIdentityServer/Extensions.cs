using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarSharingApp.Infrastructure.DuendeIdentityServer
{
    public static class Extensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Customer", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });

                options.AddPolicy("Administrator", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });
            });

            return services;
        }
    }
}
