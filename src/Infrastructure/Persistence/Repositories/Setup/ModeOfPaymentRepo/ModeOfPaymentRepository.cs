using AutoMapper;
using DMS.Application.Interfaces.Setup.ModeOfPaymentRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.ModeOfPaymentDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ModeOfPaymentRepo
{
    public class ModeOfPaymentRepository :IModeOfPaymentRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ModeOfPayment> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public ModeOfPaymentRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ModeOfPayment>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<IEnumerable<ModeOfPaymentModel>> GetAllAsync() =>
            await _db.LoadDataAsync<ModeOfPaymentModel, dynamic>("spModeOfPayment_GetAll", new { });

        public async Task<ModeOfPaymentModel?> GetAsync(int id) =>
            await _db.LoadSingleAsync<ModeOfPaymentModel, dynamic>("spModeOfPayment_Get", new { id });

        public async Task<ModeOfPayment> SaveAsync(ModeOfPaymentModel model)
        {
            var modeOfPayment = _mapper.Map<ModeOfPayment>(model);

            if (model.Id == 0)
                modeOfPayment = await CreateAsync(modeOfPayment);
            else
                modeOfPayment = await UpdateAsync(modeOfPayment);

            return modeOfPayment;
        }

        public async Task<ModeOfPayment> CreateAsync(ModeOfPayment modeOfPayment)
        {
            try
            {
                modeOfPayment.CreatedById = _currentUserService.GetCurrentUserId();
                modeOfPayment.DateCreated = DateTime.Now;
                var result = await _contextHelper.CreateAsync(modeOfPayment, "");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ModeOfPayment> UpdateAsync(ModeOfPayment modeOfPayment)
        {
            try
            {
                var result = await _contextHelper.UpdateAsync(modeOfPayment, "CreatedById", "DateCreated");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
