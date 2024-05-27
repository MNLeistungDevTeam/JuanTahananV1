using AutoMapper;
using DMS.Domain.Dto.UserDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Application.Services;
using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.UserRepository
{
    public class UserDocumentRepository : IUserDocumentRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<UserDocument> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public UserDocumentRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<UserDocument>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }
        public async Task<UserDocument?> GetByIdAsync(int id) =>
        await _context.UserDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        public async Task<List<UserDocument>?> GetAllAsync() =>
        await _context.UserDocuments.AsNoTracking().ToListAsync();
        public async Task<UserDocument?> GetByDocumentIdAsync(int DocumentId) =>
        await _context.UserDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.DocumentId == DocumentId);
        public async Task<UserDocument> SaveAsync(UserDocumentModel model)
        {
            if (model.Id == 0)
                model = _mapper.Map<UserDocumentModel>(await CreateAsync(model));
            else
                model = _mapper.Map<UserDocumentModel>(await UpdateAsync(model));
            return _mapper.Map<UserDocument>(model);
        }
        public async Task<UserDocument> CreateAsync(UserDocumentModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<UserDocument>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified", "ModifiedById");
            return mapped;
        }
        public async Task<UserDocument> UpdateAsync(UserDocumentModel model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<UserDocument>(model);
            mapped = await _contextHelper.UpdateAsync(mapped, "DateCreated", "CreatedById");
            return mapped;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _contextHelper.GetByIdAsync(id);
            if (entity != null)
                await _contextHelper.DeleteAsync(entity);
        }
        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.UserDocuments.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }

        }
    }
}
