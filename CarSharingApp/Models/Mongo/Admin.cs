using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CarSharingApp.Models.Mongo
{
    public class Admin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired] public string FirstName { get; set; } = null!;
        [BsonRequired] public string LastName { get; set; } = null!;
        [BsonRequired] public string Username { get; set; } = null!;
        [BsonRequired] public string PhoneNumber { get; set; } = null!;
    }
}
