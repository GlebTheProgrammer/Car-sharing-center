namespace CarSharingApp.Services.Interfaces
{
    public interface IFileUploadService
    {
        public Task<string> UploadFileAsync(IFormFile file);
    }
}
