using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CarSharingApp.Models.Mongo.Includes;

namespace CarSharingApp.Models.Mongo
{
    public class Vehicle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? OwnerId { get; set; }

        [BsonRequired] public string Name { get; set; } = null!;
        [BsonRequired] public string BriefDescription { get; set; } = null!;
        [BsonRequired] public string Description { get; set; } = null!;
        [BsonRequired] public Tariff Tariff { get; set; } = null!;
        [BsonRequired] public string Image { get; set; } = null!;
        [BsonRequired] public Location Location { get; set; } = null!;
        [BsonRequired] public int TimesOrdered { get; set; } = 0;
        [BsonRequired] public DateTime PublishedTime { get; set; } = DateTime.Now;
        [BsonRequired] public DateTime? LastTimeOrdered = null!;
        [BsonRequired] public bool IsPublished { get; set; } = false;
        [BsonRequired] public bool IsOrdered { get; set; } = false;
    }
}
