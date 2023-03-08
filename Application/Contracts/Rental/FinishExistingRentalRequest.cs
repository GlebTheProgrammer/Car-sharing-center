namespace CarSharingApp.Application.Contracts.Rental
{
    public sealed record FinishExistingRentalRequest(
        string rentalId
    );
}
