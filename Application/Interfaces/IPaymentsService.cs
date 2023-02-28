using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface IPaymentsService
    {
        Task<ErrorOr<Payment>> GetByRentalIdAsync(Guid rentalId);

        ErrorOr<Payment> From(CreateNewRentalRequest request);
    }
}
