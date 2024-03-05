using DMS.Domain.Dto.DocumentDto;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.DocumentRepository;

public interface IDocumentTypeRepository
{
    Task<DocumentType?> GetByIdAsync(int id);
    Task<List<DocumentTypeModel>> SpGetAllUserDocumentTypes();
    Task<DocumentType> SaveAsync(DocumentTypeModel model);
    Task<DocumentType> CreateAsync(DocumentTypeModel model);
    Task<DocumentType> UpdateAsync(DocumentTypeModel model);
    Task DeleteAsync(int id);
    Task BatchDeleteAsync(int[] ids);
    Task<List<DocumentInfo>> SpGetAllDocumentsByIds(int ApplicationId, int DocumentTypeId);
}