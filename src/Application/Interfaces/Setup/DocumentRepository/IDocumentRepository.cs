using DMS.Domain.Dto.DocumentDto;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.DocumentRepository;

public interface IDocumentRepository
{
    Task DeleteAsync(Document document);

    Task<List<Document>> GetByIds(int[] ids);

    Task BatchDeleteAsync(int[] ids);

    Task<Document> CreateAsync(Document document);

    Task DeleteAsync(int id);

    Task<List<Document>> GetAll();

    Task<List<ApplicationSubmittedDocumentModel>> SpGetAllApplicationSubmittedDocuments(int ApplicationId);

    Task<Document?> GetById(int id);

    Task<Document?> GetCode(int refId, string refNo, int documentType);

    Task<Document> SaveAsync(Document document);

    Task<Document> UpdateAsync(Document document);
    Task<IEnumerable<DocumentModel?>> GetApplicantDocumentsByCode(string applicantCode);
}