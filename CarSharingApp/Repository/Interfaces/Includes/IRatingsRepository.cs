using CarSharingApp.Models.RatingData;

namespace CarSharingApp.Repository.Interfaces.Includes
{
    public interface IRatingsRepository
    {
        public Task<int> CreateNewVehicleRating();
        public RatingModel GetVehicleRatingById(int id);
        public void DeleteVehicleRating(int ratingId);

        public void UpdateVehicleRating(int ratingId, ProvideRatingViewModel userVehicleRating);

        public Task SaveChanges();
    }
}
