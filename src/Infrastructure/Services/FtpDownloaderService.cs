
using DMS.Domain.Dto.FtpServerConfigDto;
using Microsoft.Extensions.Options;
using System.Net;
using DMS.Application.Services;

namespace DMS.Infrastructure.Services
{
    public class FtpDownloaderService : IFtpDownloaderService
    {
        private readonly FtpServerConfigModel _ftpServerConfig;
        private readonly HttpClient _httpClient;

        public FtpDownloaderService(IOptions<FtpServerConfigModel> ftpServerConfig, HttpClient httpClient)
        {
            _ftpServerConfig = ftpServerConfig.Value;
            _httpClient = httpClient;
        }

        public async Task<Stream> Download(string URLfile, Action<int>? progressCallback = null)
        {
            try
            {
                string ftpUrl = $"{_ftpServerConfig.FtpHost}/{URLfile}";
                FtpWebRequest sizeRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                sizeRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                sizeRequest.Credentials = new NetworkCredential(_ftpServerConfig.FtpUser, _ftpServerConfig.FtpPass);

                long fileSize;
                using (FtpWebResponse sizeResponse = (FtpWebResponse)sizeRequest.GetResponse())
                {
                    fileSize = sizeResponse.ContentLength;
                }

                FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                downloadRequest.Credentials = new NetworkCredential(_ftpServerConfig.FtpUser, _ftpServerConfig.FtpPass);

                using (FtpWebResponse response = (FtpWebResponse)downloadRequest.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                {
                    MemoryStream memoryStream = new MemoryStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    long totalBytesRead = 0;

                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await memoryStream.WriteAsync(buffer, 0, bytesRead);
                        totalBytesRead += bytesRead;

                        if (fileSize > 0)
                        {
                            int progress = (int)(((double)totalBytesRead / fileSize) * 100);
                            progressCallback?.Invoke(progress);
                        }
                    }
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Downloading Error!", ex);
            }
        }

        public async Task Upload(Stream fileStream, string destinationFileName, Action<int>? progressCallback = null)
        {
            try
            {
                // Ensure the stream is positioned at the beginning
                fileStream.Seek(0, SeekOrigin.Begin);

                string ftpUrl = $"{_ftpServerConfig.FtpHost}/{destinationFileName}";

                // Extract directory path from the destination file name
                string directoryPath = Path.GetDirectoryName(destinationFileName);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    // Check if directory exists, create if not
                    await CreateDirectoryIfNotExist(directoryPath);
                }

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_ftpServerConfig.FtpUser, _ftpServerConfig.FtpPass);

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    long totalBytesRead = 0;
                    long fileSize = fileStream.Length;

                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await requestStream.WriteAsync(buffer, 0, bytesRead);
                        totalBytesRead += bytesRead;

                        if (fileSize > 0)
                        {
                            int progress = (int)(((double)totalBytesRead / fileSize) * 100);
                            progressCallback?.Invoke(progress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Uploading Error!", ex);
            }
        }

        private async Task CreateDirectoryIfNotExist(string directoryPath)
        {
            try
            {
                string ftpUrl = $"{_ftpServerConfig.FtpHost}/{directoryPath}";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(_ftpServerConfig.FtpUser, _ftpServerConfig.FtpPass);

                // This will throw a WebException if the directory already exists
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    // Directory created successfully
                }
            }
            catch (WebException ex)
            {
                // Check if the directory already exists (code 550)
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    throw;
                }
            }
        }



        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                string ftpUrl = $"{_ftpServerConfig.FtpHost}/{filePath}";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(_ftpServerConfig.FtpUser, _ftpServerConfig.FtpPass);

                using (FtpWebResponse response = (FtpWebResponse)(await request.GetResponseAsync()))
                {
                    Console.WriteLine($"File deleted: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                Console.WriteLine($"Error deleting file: {response.StatusCode} - {response.StatusDescription}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
