namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IAzureBlobStoragePublicApiClient
    {
        //Task<BlobInfo> GetBlobAsync(string name);
        Task<IEnumerable<string>> ListBlobsAsync();
        Task UploadFileBlobAsync(string filePath, string fileName);
        Task UploadContentBlobAsync(string content, string fileName);
        Task UploadIFormFileBlobAsync(IFormFile file, string fileName);
        Task DeleteBlobAsync(string blobName);
    }
}
