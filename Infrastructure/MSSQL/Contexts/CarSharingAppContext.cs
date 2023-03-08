using CarSharingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarSharingApp.Infrastructure.MSSQL.Contexts
{
    public sealed class CarSharingAppContext : DbContext
    {
        public CarSharingAppContext(DbContextOptions<CarSharingAppContext> options)
            : base(options)
        {
        }

        public DbSet<ActionNote> ActionNotes { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
