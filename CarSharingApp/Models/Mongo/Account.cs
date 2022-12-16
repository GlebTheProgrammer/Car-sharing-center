using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CarSharingApp.Models.Mongo
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? CustomerId { get; set; } = null!;

        [BsonRequired] public int VehiclesOrdered { get; set; } = 0;
        [BsonRequired] public int VehiclesShared { get; set; } = 0;
        [BsonRequired] public string AccountDescription { get; set; } = "No description yet";
        [BsonRequired] public string CustomerImage { get; set; } = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTdmrjoiXGVFEcd1cX9Arb1itXTr2u8EKNpw&usqp=CAU";
    }
}
