using Microsoft.AspNetCore.Http;

namespace DMS.Application.Services;

public interface IFileUploadService
{
    Task UploadFiles(
            List<IFormFile> files,
            string rootPath,
            string saveLocation,
            int ApplicantsPersonalInformationId,
            int DocumentTypeId,
            int DocumentId);

    Task DeleteFile(int documentId, string rootFolder);

    Task BatchDeleteFile(int[] documentIds, string rootFolder);

    Task<string?> SaveFileAsync(IFormFile? file, string location, string rootPath);

    Task<string?> SaveProfilePictureAsync(IFormFile? file, string userName, string location, string rootPath);

    Task DeleteFileAsync(int documentId, string rootFolder);

    Task BatchDeleteFileAsync(int[] documentIds, string rootFolder);

    Task UploadFilesAsync(List<IFormFile>? files, string saveLocation, string rootPath, int referenceId, string referenceNo, int referenceType, int documentTypeId, int userId, int companyId);
}