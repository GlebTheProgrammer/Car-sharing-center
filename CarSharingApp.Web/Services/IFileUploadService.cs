namespace CarSharingApp.Web.Services
{
    public interface IFileUploadService
    {
        public Task<string> UploadFileAsync(IFormFile file);
    }
}
