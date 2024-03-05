

namespace DMS.Application.Services
{
    public interface IFtpDownloaderService
    {
        Task<Stream> Download(string URLfile, Action<int>? progressCallback = null);
        Task Upload(Stream fileStream, string destinationFileName, Action<int>? progressCallback = null);
        Task DeleteFileAsync(string filePath);
    }
}
