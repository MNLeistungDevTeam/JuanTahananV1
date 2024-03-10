using AutoMapper;
using DMS.Application.Interfaces.Setup.PurposeOfLoanRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.ModeOfPaymentDto;
using DMS.Domain.Dto.PurposeOfLoanDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.PurposeOfLoanRepo
{
    public class PurposeOfLoanRepository : IPurposeOfLoanRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<PurposeOfLoan> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public PurposeOfLoanRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<PurposeOfLoan>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }



        public async Task<IEnumerable<PurposeOfLoanModel>> GetAllAsync() =>
            await _db.LoadDataAsync<PurposeOfLoanModel, dynamic>("spPurposeOfLoan_GetAll", new { });

        public async Task<PurposeOfLoanModel?> GetAsync(int id) =>
            await _db.LoadSingleAsync<PurposeOfLoanModel, dynamic>("spPurposeOfLoan_Get", new { id });

        public async Task<PurposeOfLoan> SaveAsync(PurposeOfLoanModel model)
        {
            var _loanPupose = _mapper.Map<PurposeOfLoan>(model);

            if (model.Id == 0)
                _loanPupose = await CreateAsync(_loanPupose);
            else
                _loanPupose = await UpdateAsync(_loanPupose);

            return _loanPupose;
        }

        public async Task<PurposeOfLoan> CreateAsync(PurposeOfLoan loanPurpose)
        {
            try
            {
                loanPurpose.CreatedById = _currentUserService.GetCurrentUserId();
                loanPurpose.DateCreated = DateTime.Now;
                var result = await _contextHelper.CreateAsync(loanPurpose, "ModifiedById", "DateModified");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PurposeOfLoan> UpdateAsync(PurposeOfLoan loanPurpose)
        {
            try
            {
                //loanPurpose.ModifiedById =_currentUserService.GetCurrentUserId();
                //loanPurpose.DateModified = DateTime.Now;
                var result = await _contextHelper.UpdateAsync(loanPurpose, "CreatedById", "DateCreated");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}