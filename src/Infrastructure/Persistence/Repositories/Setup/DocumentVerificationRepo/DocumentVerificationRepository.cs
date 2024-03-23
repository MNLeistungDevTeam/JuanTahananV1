using AutoMapper;
using DMS.Application.Interfaces.Setup.DocumentVerification;
using DMS.Application.Services;
using DMS.Domain.Dto.DocumentVerificationDto;
using DMS.Domain.Entities;
using DMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.DocumentVerificationRepo
{
    public class DocumentVerificationRepository : IDocumentVerificationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<DocumentVerification> _contextHelper;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public DocumentVerificationRepository(DMSDBContext context, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<DocumentVerification>(context);
            _mapper = mapper;
            _db = db;
        }

        public async Task<IEnumerable<DocumentVerificationModel?>> GetByTypeAsync(int type) =>
         await _db.LoadDataAsync<DocumentVerificationModel, dynamic>("spDocumentVerification_GetByType", new { type });

        public async Task<DocumentVerification?> GetByIdAsync(int id)
        {
            return await _contextHelper.GetByIdAsync(id);
        }

        public async Task<List<DocumentVerification>> GetAllAsync()
        {
            return await _contextHelper.GetAllAsync();
        }

        public async Task<DocumentVerification> SaveAsync(DocumentVerificationModel model, int userId)
        {
            var documentVerification = _mapper.Map<DocumentVerification>(model);

            if (model.Id == 0)
            {
                documentVerification = await CreateAsync(documentVerification, userId);
            }
            else
                documentVerification = await UpdateAsync(documentVerification, userId);

            return documentVerification;
        }

        public async Task<DocumentVerification> CreateAsync(DocumentVerification documentVerification, int userId)
        {
            documentVerification.CreatedById = userId;
            documentVerification.DateCreated = DateTime.Now;
            var result = await _contextHelper.CreateAsync(documentVerification, "ModifiedById", "DateModified");
            return result;
        }

 

        public async Task<DocumentVerification> UpdateAsync(DocumentVerification documentVerification, int userId)
        {
            documentVerification.ModifiedById = userId;
            documentVerification.DateModified = DateTime.Now;
            var result = await _contextHelper.UpdateAsync(documentVerification, "CreatedById", "DateCreated");

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var document = await _context.DocumentVerifications.FindAsync(id);

            if (document == null)
                return;

            await _contextHelper.DeleteAsync(document);
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var documents = await _context.DocumentVerifications.Where(d => ids.Contains(d.Id)).ToListAsync();
            await _contextHelper.BatchDeleteAsync(documents);
        }
    }
}