using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.ValidationErrors;
using CarSharingApp.Domain.ValueObjects;
using ErrorOr;
using static CarSharingApp.Domain.Enums.FlagEnums;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Vehicle : Entity
    {
        public const int MinNameLength = 3;
        public const int MaxNameLength = 30;
        public const int MinBriefDescriptionLength = 30;
        public const int MaxBriefDescriptionLength = 80;
        public const int MinDescriptionLength = 50;
        public const int MaxDescriptionLength = 300;

        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }
        public string Image { get; private set; }
        public string BriefDescription { get; private set; }
        public string Description { get; private set; }
        public Tariff Tariff { get; private set; }
        public Location Location { get; private set; }
        public int TimesOrdered { get; private set; }
        public DateTime PublishedTime { get; private set; }
        public DateTime? LastTimeOrdered { get; private set; }
        public Status Status { get; private set; }
        public Specifications Specifications { get; private set; }
        public Categories Categories { get; private set; }

        private Vehicle(
            Guid id,
            Guid customerId,
            string name, 
            string image, 
            string briefDescription, 
            string description, 
            Tariff tariff, 
            Location location, 
            int timesOrdered, 
            DateTime publishedTime, 
            DateTime? lastTimeOrdered, 
            Status status,
            Specifications specifications,
            Categories category)
            : base(id)
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
            Status = status;
            Specifications = specifications;  
            Categories = category;
        }

        public static ErrorOr<Vehicle> Create(
            Guid customerId,
            string name,
            string image,
            string briefDescription,
            string description,
            decimal hourlyRentalPrice,
            decimal dailyRentalPrice,
            string streetAddress,
            string aptSuiteEtc,
            string city,
            string country,
            string latitude,
            string longitude,
            int productionYear,
            int maxSpeedKph,
            string exteriorColor,
            string interiorColor,
            string drivetrain,
            string fuelType,
            string transmission,
            string engine,
            string vin,
            List<string> categories,
            Guid? id = null,
            bool isPublished = false,
            bool isOrdered = false,
            bool isConfirmedByAdmin = false,
            int timesOrdered = 0,
            DateTime? publishedTime = null,
            DateTime? lastTimeOrdered = null)
        {
            List<Error> errors = new();

            if (name.Length is < MinNameLength or > MaxNameLength)
            {
                errors.Add(DomainErrors.Vehicle.InvalidName);
            }
            if (briefDescription.Length is < MinBriefDescriptionLength or > MaxBriefDescriptionLength)
            {
                errors.Add(DomainErrors.Vehicle.InvalidBriefDescription);
            }
            if (description.Length is < MinDescriptionLength or > MaxDescriptionLength)
            {
                errors.Add(DomainErrors.Vehicle.InvalidDescription);
            }

            ErrorOr<Location> locationCreateRequest = Location.Create(streetAddress, aptSuiteEtc, city, country, latitude, longitude);
            if (locationCreateRequest.IsError) 
            {
                errors.AddRange(locationCreateRequest.Errors); 
            }
            ErrorOr<Tariff> tariffCreateRequest = Tariff.Create(hourlyRentalPrice, dailyRentalPrice);
            if (tariffCreateRequest.IsError)
            {
                errors.AddRange(tariffCreateRequest.Errors);
            }
            ErrorOr<Specifications> specificationsCreateRequest = Specifications.Create(productionYear, maxSpeedKph, exteriorColor, 
                                                                                        interiorColor, drivetrain, fuelType, transmission, engine, vin);
            if (specificationsCreateRequest.IsError)
            {
                errors.AddRange(specificationsCreateRequest.Errors);
            }
            ErrorOr<Status> statusCreateRequest = Status.Create(isConfirmedByAdmin, isPublished, isOrdered);
            if (statusCreateRequest.IsError)
            {
                errors.AddRange(statusCreateRequest.Errors);
            }
            ErrorOr<Categories> categoriesCreateRequest = GetCategoriesFromList(categories);
            if (categoriesCreateRequest.IsError)
            {
                errors.AddRange(categoriesCreateRequest.Errors);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Vehicle(
                id ?? Guid.NewGuid(),
                customerId,
                name,
                image,
                briefDescription,
                description,
                tariffCreateRequest.Value,
                locationCreateRequest.Value,
                timesOrdered,
                publishedTime ?? DateTime.UtcNow,
                lastTimeOrdered ?? null,
                statusCreateRequest.Value,
                specificationsCreateRequest.Value,
                categoriesCreateRequest.Value);
        }
    }
}
