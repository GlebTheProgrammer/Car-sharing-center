using CarSharingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarSharingApp.Infrastructure.MSSQL.Contexts
{
    public class CarSharingAppContext : DbContext
    {
        public CarSharingAppContext(DbContextOptions<CarSharingAppContext> options)
            : base(options)
        {
        }

        public DbSet<ActionNote> ActionNotes { get; set; }
    }
}
