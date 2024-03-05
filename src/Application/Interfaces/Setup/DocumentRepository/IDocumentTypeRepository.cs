using Template.Domain.Dto.DocumentDto;
using Template.Domain.Entities;

namespace Template.Application.Interfaces.Setup.DocumentRepository;

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