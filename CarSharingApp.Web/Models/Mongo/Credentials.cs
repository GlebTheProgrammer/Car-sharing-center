using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CarSharingApp.Models.Mongo
{
    public class Credentials
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; } = null!;

        [BsonRequired] public string Email { get; set; } = null!;
        [BsonRequired] public string Password { get; set; } = null!;
    }
}
