using Template.Domain.Dto.DocumentDto;
using Template.Domain.Entities;

namespace Template.Application.Interfaces.Setup.DocumentRepository;

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
    Task<Document?> GetCode(int refId,string refNo,int documentType);
    Task<Document> SaveAsync(Document document);

    Task<Document> UpdateAsync(Document document);
}