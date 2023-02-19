using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;

namespace CarSharingApp.Application.Services
{
    public class RentalsService : IRentalsService
    {
        private readonly IRepository<Rental> _rentalsRepository;

        public RentalsService()
        {

        }
    }
}
