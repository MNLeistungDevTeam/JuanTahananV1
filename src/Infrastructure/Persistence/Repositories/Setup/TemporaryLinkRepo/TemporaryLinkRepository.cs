using AutoMapper;
using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.TemporaryLinkRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.TemporaryLinkDto;
using DMS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.TemporaryLinkRepo
{
    public class TemporaryLinkRepository : ITemporaryLinkRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<TemporaryLink> _contextHelper;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;
        private readonly ICompanyRepository _companyRepo;
        private readonly IModuleRepository _moduleRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TemporaryLinkRepository(
            DMSDBContext context,
            IMapper mapper,
            ISQLDatabaseService db,
            ICompanyRepository companyRepo,
            IModuleRepository moduleRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<TemporaryLink>(context);
            _mapper = mapper;
            _db = db;

            _companyRepo = companyRepo;
            _moduleRepo = moduleRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Getters

        public async Task<List<TemporaryLink>> GetAllAsync() =>
            await _contextHelper.GetAllAsync();

        public async Task<TemporaryLink?> GetByIdAsync(int id) =>
            await _contextHelper.GetByIdAsync(id);

        public async Task<TemporaryLink?> GetByGuId(string guid)
        {
            var temp = await _context.TemporaryLinks.FirstOrDefaultAsync(x => x.GuId.ToString() == guid && x.IsSubmitted == false);
            return temp;
        }

        #endregion Getters

        #region Operation

        public async Task<TemporaryLink> CreateAsync(TemporaryLink model)
        {

            var result = await _contextHelper.CreateAsync(model);

            return result;
        }

        public async Task<TemporaryLink> UpdateAsync(TemporaryLink model)
        {
            var result = await _contextHelper.UpdateAsync(model);

            return result;
        }

        public async Task<TemporaryLink> SaveAsync(TemporaryLinkModel model)
        {
            var _temporarylink = _mapper.Map<TemporaryLink>(model);
            if (model.Id == 0)
            {
                _temporarylink = await CreateAsync(_temporarylink);
            }
            else
            {
                _temporarylink = await UpdateAsync(_temporarylink);
            }

            return _temporarylink;
        }
        public async Task<TemporaryLink> SaveContextAsync(TemporaryLink model)
        {
            var _temporarylink = model;
            if (model.Id == 0)
            {
                _temporarylink = await CreateAsync(model);
            }
            else
            {
                _temporarylink = await UpdateAsync(model);
            }

            return _temporarylink;
        }

        public async Task DeleteByBatch(int[] ids)
        {
            var entity = _context.TemporaryLinks.
                Where(x => ids.Contains(x.Id)).ToList();

            if (entity is not null)
                await _contextHelper.BatchDeleteAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _context.TemporaryLinks.
                Where(x => x.Id == id).FirstOrDefault();

            if (entity is not null)
                await _contextHelper.DeleteAsync(entity);
        }

        #endregion Operation

        #region Public Methods
        public async Task<TemporaryLink>? GetTemporaryLinkData(TemporaryLinkModel model)
        {
            var link = await _context.TemporaryLinks.FirstOrDefaultAsync(x =>
            x.GuId == model.GuId
            && (model.UserId == null || x.UserId == model.UserId));

            if (link == null)
            {
                var templink = new TemporaryLink()
                {
                    Id = 0,
   
                    UserId = model.UserId,
             
                    GuId = model.GuId,
                };

                return templink;
            }
            else
            {
                return link;
            }
        }

        public async Task<TemporaryLink>? UpdateSessionOnOpen(TemporaryLink linkData)
        {
            try
            {
                var defaultData = await _context.TemporaryLinks.FirstOrDefaultAsync(x => x.GuId == linkData.GuId);
                if ((bool)defaultData.IsSubmitted)
                {
                    throw new Exception("Form already submitted.");
                }
                defaultData.IsOpened = true;
                defaultData.DateExpiration = DateTime.Now.AddHours(4);
                var updatedData = await UpdateAsync(defaultData);
                return (updatedData);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }


        public void SetSession(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        public string GetSession(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }
        public void RemoveSession(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods
    }
}
