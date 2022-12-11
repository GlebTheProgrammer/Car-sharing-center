using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CarSharingApp.Models.Mongo
{
    public class Rating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? VehicleId { get; set; }

        [BsonRequired] public double Condition { get; set; } = 0;
        [BsonRequired] public double FuelConsumption { get; set; } = 0;
        [BsonRequired] public double EasyToDrive { get; set; } = 0; 
        [BsonRequired] public double FamilyFriendly { get; set; } = 0;
        [BsonRequired] public double SUV { get; set; } = 0;
        [BsonRequired] public double Overall { get; set; } = 0;
        [BsonRequired] public int ReviewsCount { get; set; } = 0;
    }
}
