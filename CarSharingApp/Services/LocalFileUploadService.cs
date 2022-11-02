using CarSharingApp.Services.Interfaces;

namespace CarSharingApp.Services
{
    public class LocalFileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment environment;

        public LocalFileUploadService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            DateTime dateTime = DateTime.Now;
            string newFileName = $"{dateTime.Hour}_{dateTime.Minute}_{dateTime.Second}_{file.FileName}";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/vehicleImages/", newFileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return Path.Combine("/vehicleImages/", newFileName);
        }
    }
}
