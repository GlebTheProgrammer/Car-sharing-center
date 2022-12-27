using CarSharingApp.Models.Mongo;
using CarSharingApp.Models.MongoView;

namespace CarSharingApp.Services
{
    public static class RatingCalculationProviderService
    {
        public static Rating CalculateNewRating(Rating currentRating, RatingSubmitModel submittedRating)
        {
            currentRating.SUV = Calculate(currentRating.SUV, currentRating.ReviewsCount, submittedRating.SUV);
            currentRating.FamilyFriendly = Calculate(currentRating.FamilyFriendly, currentRating.ReviewsCount, submittedRating.FamilyFriendly);
            currentRating.FuelConsumption = Calculate(currentRating.FuelConsumption, currentRating.ReviewsCount, submittedRating.FuelConsumption);
            currentRating.Condition = Calculate(currentRating.Condition, currentRating.ReviewsCount, submittedRating.Condition);
            currentRating.EasyToDrive = Calculate(currentRating.EasyToDrive, currentRating.ReviewsCount, submittedRating.EasyToDrive);

            currentRating.Overall = (currentRating.SUV + currentRating.FamilyFriendly + currentRating.FuelConsumption + currentRating.Condition + currentRating.EasyToDrive) / 5;

            currentRating.ReviewsCount += 1;

            return currentRating;
        }

        private static double Calculate(double current, int reviews, double submitted)
        {
            return (current * reviews + submitted) / (reviews + 1);
        }
    }
}
