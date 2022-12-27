namespace CarSharingApp.Models.MongoView
{
    public class RentalAccountModel
    {

        public string? Id { get; set; }
        public string? RentedVehicleId { get; set; }

        public string? RentedVehicleName { get; set; } = null!;
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int RentalTimeInMinutes { get; set; }

    }
}
