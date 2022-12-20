using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarSharingApp.Models.Mongo
{
    public class Rental
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? CustomerId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? RentedVehicleId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? VehicleOwnerId { get; set; }

        [BsonRequired] public string? RentedVehicleName { get; set; } = null!;
        [BsonRequired] public DateTime RentalDate { get; set; } = DateTime.Now;
        [BsonRequired] public int RentalTimeInMinutes { get; set; }
        [BsonRequired] public DateTime ReturnDate { get; set; }
        [BsonRequired] public bool IsActive { get; set; } = true;
    }
}
