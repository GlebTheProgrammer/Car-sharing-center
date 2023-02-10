using CarSharingApp.Infrastructure.MSSQL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CarSharingApp.Infrastructure.MSSQL.Seeds
{
    public class CarSharingAppContextSeed
    {
        public static async Task SeedAsync(CarSharingAppContext carSharingAppContext,
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
            catch (Exception)
            {
                if (retryForAvailability >= 10) throw;

                retryForAvailability++;

                await SeedAsync(carSharingAppContext, retryForAvailability);
                throw;
            }
        }
    }
}
