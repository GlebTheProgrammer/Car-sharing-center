using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Vehicle : Entity
    {
        public string Name { get; private set; }
        public string Image { get; private set; }
        public string BriefDescription { get; private set; }
        public string Description { get; private set; }
        public Tariff Tariff { get; private set; }
        public Location Location { get; private set; }
        public int TimesOrdered { get; private set; }
        public DateTime PublishedTime { get; private set; }
        public DateTime LastTimeOrdered { get; private set; }
        public bool IsPublished { get; private set; }
        public bool IsOrdered { get; private set; }
        public Specifications Specifications { get; private set; }

        public Guid CustomerId { get; private set; } // many:1
        public Customer? Customer { get; private set; }
        public List<Review> Reviews { get; private set; } = new(); // 1:many

        public Vehicle(Guid customerId,
            string name, 
            string image, 
            string briefDescription, 
            string description, 
            Tariff tariff, 
            Location location, 
            int timesOrdered, 
            DateTime publishedTime, 
            DateTime lastTimeOrdered, 
            bool isPublished, 
            bool isOrdered,
            Specifications specifications)
        {
            CustomerId = customerId;

            Name = name;
            Image = image;
            BriefDescription = briefDescription;
            Description = description;
            Tariff = tariff;
            Location = location;
            TimesOrdered = timesOrdered;
            PublishedTime = publishedTime;
            LastTimeOrdered = lastTimeOrdered;
            IsPublished = isPublished;
            IsOrdered = isOrdered;
            Specifications = specifications;
        }
    }
}
