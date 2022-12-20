using CarSharingApp.Models.Mongo.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarSharingApp.Models.Mongo
{
    public class Specifications
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? VehicleId { get; set; }

        [BsonRequired] public int ProductionYear { get; set; }
        [BsonRequired] public int MaxSpeed { get; set; }
        [BsonRequired] public string ExteriorColor { get; set; } = null!;
        [BsonRequired] public string InteriorColor { get; set; } = null!;
        [BsonRequired] public Drivetrain Drivetrain { get; set; }
        [BsonRequired] public FuelType FuelType { get; set; }
        [BsonRequired] public Transmission Transmission { get; set; }
        [BsonRequired] public Engine Engine { get; set; }
        [BsonRequired] public string VIN { get; set; } = null!;
    }
}
