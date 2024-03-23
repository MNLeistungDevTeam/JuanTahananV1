using DMS.Domain.Dto.DocumentDto;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.DocumentRepository;

public interface IDocumentTypeRepository
{
    Task<DocumentType?> GetByIdAsync(int id);
    Task<List<DocumentTypeModel>> SpGetAllUserDocumentTypes();
    Task<DocumentType> SaveAsync(DocumentTypeModel model);
    Task<DocumentType> CreateAsync(DocumentType model);
    Task<DocumentType> UpdateAsync(DocumentType model);
    Task DeleteAsync(int id);
    Task BatchDeleteAsync(int[] ids);
    Task<List<DocumentInfo>> SpGetAllDocumentsByIds(int ApplicationId, int DocumentTypeId);
    Task<IEnumerable<DocumentTypeModel?>> GetByApplicantCodeAsync(string applicantCode);
    Task<IEnumerable<DocumentTypeModel?>> GetInquiryAsync();
    Task BatchDeleteAsync2(int[] ids);
}