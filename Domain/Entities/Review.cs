using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Review : Entity
    {
        public string Message { get; private set; }
        public int ReviewScore { get; private set; }
        public DateTime Date { get; private set; }

        public Guid CustomerId { get; private set; } // many:1
        public Customer? Customer { get; private set; }
        public Guid VehicleId { get; private set; } // many:1
        public Vehicle? Vehicle { get; private set; }

        public Review(Guid id,
            string message, 
            int reviewScore, 
            DateTime date, 
            Guid customerId, 
            Guid vehicleId)
            : base(id)
        {
            Message = message;
            ReviewScore = reviewScore;
            Date = date;
            CustomerId = customerId;
            VehicleId = vehicleId;
        }
    }
}
