using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using ErrorOr;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarSharingApp.Infrastructure.Middlewares
{
    public sealed class TimeOutRentalsCheckMiddleware : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimeOutRentalsCheckMiddleware> _logger;
        private Timer? _timer = null;

        private readonly IVehicleService _vehicleService;
        private readonly IRentalsService _rentalsService;

        public TimeOutRentalsCheckMiddleware(ILogger<TimeOutRentalsCheckMiddleware> logger, 
                                             IRentalsService rentalsService, 
                                             IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
            _rentalsService = rentalsService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Out Rentals Checkout Service has started at {startedDateTime}.", DateTime.UtcNow);

            _timer = new Timer(AnalyzeExpiredRentals, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void AnalyzeExpiredRentals(object? state)
        {
            var expiredRentals = await _rentalsService.GetAllExpiredAndActiveRentals();
            
            if (expiredRentals.Count > 0)
            {
                foreach (var expiredRental in expiredRentals)
                {
                    try
                    {
                        ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(expiredRental.VehicleId);

                        if (getVehicleResult.IsError)
                            throw new NullReferenceException(nameof(getVehicleResult));

                        Vehicle vehicleToUpdate = getVehicleResult.Value;

                        await _rentalsService.FinishExistingRental(expiredRental.Id, false);
                        await _vehicleService.UpdateVehicleStatusAsync(_vehicleService.From(vehicleToUpdate, new UpdateVehicleStatusRequest(
                            IsOrdered: false, IsPublished: true, IsConfirmedByAdmin: true)).Value);

                        _logger.LogInformation("Timed Out Rentals Checkout Service has finished rental with ID: {rentalId} and changed vehicle with ID: {vehicleId} status successfully.", expiredRental.Id, expiredRental.VehicleId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error: {exception} occured while finishing rental with ID: {rentalId}", ex, expiredRental.Id);
                    }
                }
            }

            var count = Interlocked.Increment(ref executionCount);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Out Rentals Checkout Service was stopped at {stoppedDateTime}.", DateTime.UtcNow);

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
