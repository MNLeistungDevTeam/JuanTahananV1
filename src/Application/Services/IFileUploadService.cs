using Microsoft.AspNetCore.Http;

namespace Template.Application.Services;

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
}