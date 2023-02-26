using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface IRentalsService
    {
        Task<Created> SubmitNewRental(Rental rental, Payment payment);
        Task FinishExistingRental(Guid rentalId, bool hasFinishedByTheCustomer);
        Task<ErrorOr<Rental>> GetRentalAsync(Guid id);
        Task<List<Rental>> GetAllCustomerRentalsAsync(Guid customerId);

        ErrorOr<Rental> From(CreateNewRentalRequest request, Guid rentedCustomerId);
    }
}
