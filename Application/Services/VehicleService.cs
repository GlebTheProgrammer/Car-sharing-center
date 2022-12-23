using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly ICustomerRepository _customerRepository;

        public VehicleService(IVehicleRepository vehicleRepository, 
            IReviewRepository reviewRepository, 
            IRentalRepository rentalRepository, 
            ICustomerRepository customerRepository)
        {
            _vehicleRepository = vehicleRepository;
            _reviewRepository = reviewRepository;
            _rentalRepository = rentalRepository;
            _customerRepository = customerRepository;
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.CreateAsync(vehicle);
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            //var result = await _vehicleRepository.GetAllAsync();

            //return result.ToList();

            var spec = new Specifications(2002, 10, Colour.Black, Colour.Blue, Domain.Enums.Drivetrain.All_wheel_drive, Domain.Enums.FuelType.Gasoline, Domain.Enums.Transmission.Automatic, Domain.Enums.Engine.Physical, "123");
            return new List<Vehicle>()
            {
                new Vehicle(new Guid(), "", "", "", "", new Domain.ValueObjects.Tariff(10, 20), new Domain.ValueObjects.Location("", "", ""), 0, DateTime.Now, DateTime.Now, false, false, spec)
            };
        }
    }
}
