using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CarSharingApp.Models.Mongo.Enums;

namespace CarSharingApp.Models.Mongo
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired] public string FirstName { get; set; } = null!;
        [BsonRequired] public string LastName { get; set; } = null!;
        [BsonRequired] public string Username { get; set; } = null!;
        [BsonRequired] public Country Country { get; set; }
        [BsonRequired] public City City { get; set; }
        [BsonRequired] public int Postcode { get; set; }
        [BsonRequired] public string Address { get; set; } = null!;
        [BsonRequired] public string PhoneNumber { get; set; } = null!;
        [BsonRequired] public string DriverLicenseIdentifier { get; set; } = null!;
        [BsonRequired] public bool IsAcceptedNewsSharing { get; set; } = true;
        [BsonRequired] public bool IsOnline { get; set; } = false;
    }
}
