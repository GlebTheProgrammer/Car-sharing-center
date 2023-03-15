using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using ErrorOr;
using CarSharingApp.Application.ServiceErrors;

namespace CarSharingApp.Application.Services
{
    public sealed class RentalsService : IRentalsService
    {
        private readonly IRepository<Rental> _rentalsRepository;
        private readonly IRepository<Payment> _paymentsRepository;
        private readonly IRepository<ActionNote> _notesRepository;
        private readonly IRepository<Vehicle> _vehiclesRepository;

        public RentalsService(IRepository<Rental> rentalsRepository, 
                              IRepository<Payment> paymentsRepository,
                              IRepository<ActionNote> notesRepository,
                              IRepository<Vehicle> vehiclesRepository)
        {
            _rentalsRepository = rentalsRepository;
            _paymentsRepository = paymentsRepository;
            _notesRepository = notesRepository;
            _vehiclesRepository = vehiclesRepository;
        }

        public async Task FinishExistingRental(Guid rentalId, bool hasFinishedByTheCustomer)
        {
            var rentalToFinish = await _rentalsRepository.GetAsync(rentalId);

            await _rentalsRepository.UpdateAsync(Rental.Finish(rentalToFinish));

            Vehicle rentedVehicle = await _vehiclesRepository.GetAsync(rentalToFinish.VehicleId);

            if (hasFinishedByTheCustomer)
                await _notesRepository.CreateAsync(
                    ActionNote.FinishedExistingRentalByYourselfNote(actorId: rentalToFinish.RentedCustomerId,
                                                                     vehicleName: rentedVehicle.Name,
                                                                     vehicleId: rentalToFinish.VehicleId));
            else
                await _notesRepository.CreateAsync(
                    ActionNote.FinishedExistingRentalBySystemNote(actorId: rentalToFinish.RentedCustomerId,
                                                                  vehicleName: rentedVehicle.Name,
                                                                  vehicleId: rentalToFinish.VehicleId));

            await _notesRepository.CreateAsync(
                ActionNote.OwnerVehicleRentalIsOverNote(actorId: rentalToFinish.VehicleOwnerId,
                                                        vehicleName: rentedVehicle.Name,
                                                        vehicleId: rentalToFinish.VehicleId));
        }

        public async Task<Created> SubmitNewRental(Rental rental, Payment payment)
        {
            // Create 1:1 Dependency
            //await _rentalsRepository.CreateAsync(rental);
            await _paymentsRepository.CreateAsync(Payment.CombinePaymentWithARental(payment, rental));

            Vehicle rentedVehicle = await _vehiclesRepository.GetAsync(rental.VehicleId);

            await _notesRepository.CreateAsync(
                ActionNote.RentedNewVehicleNote(actorId: rental.RentedCustomerId, 
                                                vehicleName: rentedVehicle.Name, 
                                                vehicleId: rental.VehicleId, 
                                                amount: payment.Amount));

            await _notesRepository.CreateAsync(
                ActionNote.OwnerVehicleRentedNote(actorId: rental.VehicleOwnerId,
                                                  vehicleName: rentedVehicle.Name,
                                                  vehicleId: rental.VehicleId,
                                                  amount: payment.Amount));

            return Result.Created;
        }

        public async Task<ErrorOr<Rental>> GetRentalAsync(Guid id)
        {
            var result = await _rentalsRepository.GetAsync(id);

            if (result != null)
                return result;
            else
                return ApplicationErrors.Rental.NotFound;
        }

        public async Task<List<Rental>> GetAllCustomerRentalsAsync(Guid customerId)
        {
            var result = await _rentalsRepository.GetAllAsync(r => r.RentedCustomerId == customerId);

            return result.ToList();
        }

        public ErrorOr<Rental> From(CreateNewRentalRequest request, Guid rentedCustomerId)
        {
            return Rental.Create(
                rentedCustomerId: rentedCustomerId,
                vehicleOwnerId: Guid.Parse(request.VehicleOwnerId),
                vehicleId: Guid.Parse(request.VehicleId),
                rentalStartsDateTime: request.RentalStartsDateTime,
                rentalEndsDateTime: request.RentalEndsDateTime);
        }

        public async Task<List<Rental>> GetAllExpiredAndActiveRentals()
        {
            var result = await _rentalsRepository.GetAllAsync(r => r.IsActive && r.RentalEndsDateTime < DateTime.UtcNow);

            return result.ToList();
        }
    }
}
