using CarSharingApp.Models.RatingData;

namespace CarSharingApp.Repository.Interfaces
{
    public interface IRatingRepository
    {
        public Task<int> CreateNewVehicleRating();
        public RatingModel GetVehicleRatingById(int id);
        public void DeleteVehicleRating(int ratingId);

        public Task UpdateVehicleRating(int ratingId, ProvideRatingViewModel userVehicleRating);

        public Task SaveChanges();
    }
}
