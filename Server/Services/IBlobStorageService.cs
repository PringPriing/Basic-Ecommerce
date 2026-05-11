namespace Ecommerce.Server.Services;

public interface IBlobStorageService
{
    Task<(string Url, string BlobName)> UploadAsync(Stream stream, string fileName, string contentType);
    Task DeleteAsync(string blobName);
}
