using AutoMapper;
using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.ModuleTypeRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.BeneficiaryInformationDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.BeneficiaryInformationRepo
{
    public class BeneficiaryInformationRepository : IBeneficiaryInformationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<BeneficiaryInformation> _contextHelper;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public BeneficiaryInformationRepository(DMSDBContext context, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<BeneficiaryInformation>(context);
            _mapper = mapper;
            _db = db;
        }

        #region Get Methods

        public async Task<BeneficiaryInformation?> GetByIdAsync(int id) =>
            await _contextHelper.GetByIdAsync(id);

        public async Task<List<BeneficiaryInformation>> GetAllAsync() =>
            await _contextHelper.GetAllAsync();


        public async Task<BeneficiaryInformationModel?> GetByPagibigNumberAsync(string? pagibigNumber) =>
  await _db.LoadSingleAsync<BeneficiaryInformationModel, dynamic>("spBeneficiaryInformation_GetByPagibigNumber", new { pagibigNumber });


        #endregion Get Methods

        #region Action Methods

        public async Task<BeneficiaryInformation> SaveAsync(BeneficiaryInformationModel beneficiaryInformation, int userId)
        {
            var _beneficiaryInformation = _mapper.Map<BeneficiaryInformation>(beneficiaryInformation);

            if (_beneficiaryInformation.Id == 0)
            {
                _beneficiaryInformation = await CreateAsync(_beneficiaryInformation, userId);
            }
            else
            {
                _beneficiaryInformation = await UpdateAsync(_beneficiaryInformation, userId);
            }

            return _beneficiaryInformation;
        }

        public async Task<BeneficiaryInformation> CreateAsync(BeneficiaryInformation beneficiaryInformation, int userId)
        {
            beneficiaryInformation.CreatedById = userId;
            beneficiaryInformation.DateCreated = DateTime.Now;

            var result = await _contextHelper.CreateAsync(beneficiaryInformation, "ModifiedById", "DateModified");

            return result;
        }

        public async Task<BeneficiaryInformation> UpdateAsync(BeneficiaryInformation beneficiaryInformation, int userId)
        {
            beneficiaryInformation.ModifiedById = userId;
            beneficiaryInformation.DateModified = DateTime.Now;

            var result = await _contextHelper.UpdateAsync(beneficiaryInformation, "ModifiedById", "DateModified");

            return result;
        }

        public async Task BachDeleteAsync(int[] ids)
        {
            var beneficiaryInformations = await _context.BeneficiaryInformations
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();

            if (beneficiaryInformations is not null)
            {
                await _contextHelper.BatchDeleteAsync(beneficiaryInformations);
            }
        }

        #endregion Action Methods
    }
}