using CarSharingApp.Infrastructure.MSSQL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarSharingApp.Infrastructure.MSSQL.Seeds
{
    public sealed class CarSharingAppContextSeed
    {
        public static async Task SeedAsync(CarSharingAppContext carSharingAppContext,
                                           ILogger logger,
                                           int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (carSharingAppContext.Database.IsSqlServer())
                {
                    carSharingAppContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;

                retryForAvailability++;

                logger.LogError(ex.Message);

                await SeedAsync(carSharingAppContext, logger, retryForAvailability);
                throw;
            }
        }
    }
}
