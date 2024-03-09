using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.DocumentDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.DocumentRepository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<Document> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISQLDatabaseService _db;

        public DocumentRepository(DMSDBContext context, ICurrentUserService currentUserService, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Document>(context);
            _currentUserService = currentUserService;
            _db = db;
        }

        public async Task<List<ApplicationSubmittedDocumentModel>> SpGetAllApplicationSubmittedDocuments(int ApplicationId) =>
            (await _db.LoadDataAsync<ApplicationSubmittedDocumentModel, dynamic>("spDocument_GetAllApplicationSubmittedDocuments", new { ApplicationId })).ToList();

        public async Task<Document?> GetById(int id)
        {
            var result = await _contextHelper.GetByIdAsync(id);
            return result;
        }

        public async Task<Document?> GetCode(int refId, string refNo, int documentType)
        {
            var result = await _context.Documents.AsNoTracking().FirstOrDefaultAsync(x => x.ReferenceId == refId && x.ReferenceNo == refNo && x.DocumentTypeId == documentType);
            return result;
        }

        public async Task<List<Document>> GetByIds(int[] ids)
        {
            var result = await _context.Documents.Where(d => ids.Contains(d.Id)).ToListAsync();
            return result;
        }

        public async Task<List<Document>> GetAll()
        {
            var result = await _contextHelper.GetAllAsync();
            return result;
        }

        public async Task<IEnumerable<DocumentModel?>> GetApplicantDocumentsByCode(string applicantCode) =>
             (await _db.LoadDataAsync<DocumentModel, dynamic>("spDocument_GetApplicantDocumentsByCode", new { applicantCode }));

        public async Task<Document> SaveAsync(Document document)
        {
            if (document.Id == 0)
            {
                document = await CreateAsync(document);
            }
            else
            {
                document = await UpdateAsync(document);
            }

            return document;
        }

        public async Task<Document> CreateAsync(Document document)
        {
            document.CreatedById = _currentUserService.GetCurrentUserId();
            document.DateCreated = DateTime.UtcNow;
            var result = await _contextHelper.CreateAsync(document, "ModifiedById", "DateModified");

            return result;
        }

        public async Task<Document> UpdateAsync(Document document)
        {
            document.ModifiedById = _currentUserService.GetCurrentUserId();
            document.DateModified = DateTime.UtcNow;
            var result = await _contextHelper.CreateAsync(document, "CreatedById", "DateCreated");
            return result;
        }

        public async Task DeleteAsync(Document document)
        {
            await _contextHelper.DeleteAsync(document);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.Documents.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }
    }
}