using CarSharingApp.Models.RatingData;
using CarSharingApp.Repository.Interfaces.Includes;
using System.Text.Json;

namespace CarSharingApp.Repository.LocalRepository.Includes
{
    public sealed class RatingsLocalRepository : IRatingsRepository
    {
        private const string filePath = "~/../Repository/LocalRepository/Data/RatingData.json";

        private List<RatingModel> ratings;

        public RatingsLocalRepository()
        {
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
            var newRating = new RatingModel();

            if (ratings.Count == 0)
                newRating.Id = 0;
            else
                newRating.Id = ratings.Max(vehicle => vehicle.Id) + 1;

            ratings.Add(newRating);
            await SaveChanges();
            return newRating.Id;
        }

        public RatingModel GetVehicleRatingById(int id)
        {
            return ratings.First(rating => rating.Id == id);
        }

        public async void UpdateVehicleRating(int ratingId, ProvideRatingViewModel userVehicleRating)
        {
            int replaceIndex = ratings.IndexOf(ratings.First(rating => rating.Id == ratingId));

            ratings[replaceIndex].SUV = (ratings[replaceIndex].SUV * ratings[replaceIndex].ReviewsCount + userVehicleRating.SUV) / (ratings[replaceIndex].ReviewsCount + 1);
            ratings[replaceIndex].Condition = (ratings[replaceIndex].Condition * ratings[replaceIndex].ReviewsCount + userVehicleRating.Condition) / (ratings[replaceIndex].ReviewsCount + 1);
            ratings[replaceIndex].FuelConsumption = (ratings[replaceIndex].FuelConsumption * ratings[replaceIndex].ReviewsCount + userVehicleRating.FuelConsumption) / (ratings[replaceIndex].ReviewsCount + 1);
            ratings[replaceIndex].FamilyFriendly = (ratings[replaceIndex].FamilyFriendly * ratings[replaceIndex].ReviewsCount + userVehicleRating.FamilyFriendly) / (ratings[replaceIndex].ReviewsCount + 1);
            ratings[replaceIndex].EasyToDrive = (ratings[replaceIndex].EasyToDrive * ratings[replaceIndex].ReviewsCount + userVehicleRating.EasyToDrive) / (ratings[replaceIndex].ReviewsCount + 1);

            ratings[replaceIndex].Overall = (ratings[replaceIndex].SUV + ratings[replaceIndex].Condition + ratings[replaceIndex].FuelConsumption + ratings[replaceIndex].FamilyFriendly + ratings[replaceIndex].EasyToDrive) / 5;

            ratings[replaceIndex].ReviewsCount += 1;

            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, ratings, options);
            await createStream.DisposeAsync();
        }

        public async void DeleteVehicleRating(int ratingId)
        {
            ratings.Remove(ratings.First(rating => rating.Id == ratingId));

            await SaveChanges();
        }
    }
}
