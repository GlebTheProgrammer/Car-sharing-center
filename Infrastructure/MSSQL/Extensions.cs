using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Primitives;
using CarSharingApp.Infrastructure.MSSQL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarSharingApp.Infrastructure.MSSQL
{
    public static class Extensions
    {
        public static IServiceCollection AddMSSQLDBconnection<T>(this IServiceCollection services, T configuration) where T
            : IConfigurationBuilder, IConfigurationRoot, IDisposable
        {
            services.AddDbContext<CarSharingAppContext>(options => options.UseSqlServer
                (configuration.GetConnectionString("MSSQL")), 
                contextLifetime: ServiceLifetime.Transient,
                optionsLifetime: ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddMSSQLRepository<T>(this IServiceCollection services) where T : Entity
        {
            //services.AddSingleton<IRepository<T>, MsSqlRepository<T>>();
            services.AddTransient<IRepository<T>, MsSqlRepository<T>>();

            return services;
        }
    }
}
