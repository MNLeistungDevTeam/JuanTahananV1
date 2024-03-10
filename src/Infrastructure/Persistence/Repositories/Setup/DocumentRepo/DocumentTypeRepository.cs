using AutoMapper;
using DMS.Domain.Dto.DocumentDto;
using Microsoft.EntityFrameworkCore;
using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.Services;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.DocumentRepository
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<DocumentType> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public DocumentTypeRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<DocumentType>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<DocumentType?> GetByIdAsync(int id) =>
            await _context.DocumentTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<DocumentTypeModel>> SpGetAllUserDocumentTypes() =>
        (await _db.LoadDataAsync<DocumentTypeModel, dynamic>("spDocumentType_GetAllUserDocumentTypes", new { })).ToList();

        public async Task<List<DocumentInfo>> SpGetAllDocumentsByIds(int ApplicationId, int DocumentTypeId) =>
        (await _db.LoadDataAsync<DocumentInfo, dynamic>("spDocument_GetAllDocumentsByIds", new { ApplicationId, DocumentTypeId })).ToList();

        public async Task<IEnumerable<DocumentTypeModel?>> GetByApplicantCodeAsync(string applicantCode) =>
   await _db.LoadDataAsync<DocumentTypeModel, dynamic>("spDocumentType_GetDocumentsByApplicantInformationCode", new { applicantCode });

        public async Task<DocumentType> SaveAsync(DocumentTypeModel model)
        {
            if (model.Id == 0)
                model = _mapper.Map<DocumentTypeModel>(await CreateAsync(model));
            else
                model = _mapper.Map<DocumentTypeModel>(await UpdateAsync(model));
            return _mapper.Map<DocumentType>(model);
        }

        public async Task<DocumentType> CreateAsync(DocumentTypeModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<DocumentType>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified", "ModifiedById");
            return mapped;
        }

        public async Task<DocumentType> UpdateAsync(DocumentTypeModel model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<DocumentType>(model);
            mapped = await _contextHelper.UpdateAsync(mapped, "DateCreated", "CreatedById");
            return mapped;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _contextHelper.GetByIdAsync(id);
            if (entity != null)
            {
                entity.DateDeleted = DateTime.Now;
                entity.DeletedById = _currentUserService.GetCurrentUserId();
                if (entity is not null)
                    await _contextHelper.UpdateAsync(entity);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.DocumentTypes.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }
        }
    }
}