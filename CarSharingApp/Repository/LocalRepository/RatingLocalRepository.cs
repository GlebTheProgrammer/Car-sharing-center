using CarSharingApp.Models.RatingData;
using CarSharingApp.Repository.Interfaces;
using System.Text.Json;

namespace CarSharingApp.Repository.LocalRepository
{
    public class RatingLocalRepository : IRatingRepository
    {
        private List<RatingModel> ratings;

        private void SetUpLocalRepository()
        {
            string filePath = "~/../Repository/LocalRepository/Data/RatingData.json";

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                ratings = JsonSerializer.Deserialize<List<RatingModel>>(jsonString)!;
            }
            else
            {
                ratings = new List<RatingModel>();
            }
        }

        public async Task<int> CreateNewVehicleRating()
        {
            if (ratings == null)
                SetUpLocalRepository();

            var newRating = new RatingModel();

            if (ratings.Count == 0)
            {
                newRating.Id = 0;
            }
            else
            {
                newRating.Id = ratings.Max(vehicle => vehicle.Id) + 1;
            }

            ratings.Add(newRating);
            await SaveChanges();
            return newRating.Id;
        }

        public RatingModel GetVehicleRatingById(int id)
        {
            if (ratings == null)
                SetUpLocalRepository();

            return ratings.First(rating => rating.Id == id);
        }

        public async Task UpdateVehicleRating(int ratingId, ProvideRatingViewModel userVehicleRating)
        {
            var currentRating = ratings.First(rating => rating.Id == ratingId);

            // Here can be a mistake. Create new object or not??? (Mostly not)
            currentRating.SUV = (currentRating.SUV * currentRating.ReviewsCount + userVehicleRating.SUV) / (currentRating.ReviewsCount + 1);
            currentRating.Condition = (currentRating.Condition * currentRating.Condition + userVehicleRating.Condition) / (currentRating.ReviewsCount + 1);
            currentRating.FuelConsumption = (currentRating.FuelConsumption * currentRating.FuelConsumption + userVehicleRating.FuelConsumption) / (currentRating.ReviewsCount + 1);
            currentRating.FamilyFriendly = (currentRating.FamilyFriendly * currentRating.FamilyFriendly + userVehicleRating.FamilyFriendly) / (currentRating.ReviewsCount + 1);
            currentRating.EasyToDrive = (currentRating.EasyToDrive * currentRating.EasyToDrive + userVehicleRating.EasyToDrive) / (currentRating.ReviewsCount + 1);

            currentRating.Overall = (currentRating.SUV + currentRating.Condition + currentRating.FuelConsumption + currentRating.FamilyFriendly + currentRating.EasyToDrive) / 5;

            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            string filePath = "~/../Repository/LocalRepository/Data/RatingData.json";

            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, ratings, options);
            await createStream.DisposeAsync();
        }
    }
}
