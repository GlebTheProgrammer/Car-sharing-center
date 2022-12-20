using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CarSharingApp.Models.Mongo
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? RentalId { get; set; }

        [BsonRequired] public DateTime PaymentDate { get; set; } = DateTime.Now;
        [BsonRequired] public double Amount { get; set; }
    }
}
