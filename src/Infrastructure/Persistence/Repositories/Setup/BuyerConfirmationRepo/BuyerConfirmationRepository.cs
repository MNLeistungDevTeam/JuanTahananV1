using AutoMapper;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.BuyerConfirmationRepo
{
    public class BuyerConfirmationRepository : IBuyerConfirmationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<BuyerConfirmation> _contextHelper;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public BuyerConfirmationRepository(DMSDBContext context, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<BuyerConfirmation>(context);
            _mapper = mapper;
            _db = db;
        }

        public async Task<BuyerConfirmationModel?> GetAsync(int id) =>
            await _db.LoadSingleAsync<BuyerConfirmationModel, dynamic>("spBuyerConfirmation_Get", new { id });

        public async Task<BuyerConfirmationModel?> GetByUserAsync(int userId) =>
            await _db.LoadSingleAsync<BuyerConfirmationModel, dynamic>("spBuyerConfirmation_GetByUserId", new { userId });
    
        public async Task<BuyerConfirmationModel?> GetByCodeAsync(string code) =>
         await _db.LoadSingleAsync<BuyerConfirmationModel, dynamic>("spBuyerConfirmation_GetByCode", new { code });

        public async Task<BuyerConfirmation> SaveAsync(BuyerConfirmationModel bcModel, int userId)
        {
            var buyerConfirm = _mapper.Map<BuyerConfirmation>(bcModel);

            if (buyerConfirm.Id == 0)
            {

                buyerConfirm.Code = await GenerateBuyerConfirmationCode();

                 

                buyerConfirm = await CreateAsync(buyerConfirm, userId);
            }
            else
            {
                buyerConfirm = await UpdateAsync(buyerConfirm, userId);
            }

            return buyerConfirm;
        }

        public async Task<BuyerConfirmation> CreateAsync(BuyerConfirmation buyerConfirm, int userId)
        {
            buyerConfirm.CreatedById = userId;
            buyerConfirm.DateCreated = DateTime.Now;

            var result = await _contextHelper.CreateAsync(buyerConfirm, "DateModified", "ModifiedById");

            return result;
        }

        public async Task<BuyerConfirmation> UpdateAsync(BuyerConfirmation buyerConfirm, int userId)
        {
            buyerConfirm.ModifiedById = userId;
            buyerConfirm.DateModified = DateTime.Now;

            var result = await _contextHelper.UpdateAsync(buyerConfirm, "DateCreated", "CreatedById");
            return result;
        }

        public async Task DeleteAsync(BuyerConfirmation buyerConfirm)
        {
            await _contextHelper.DeleteAsync(buyerConfirm);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.BuyerConfirmations.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.BuyerConfirmations.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }

        public async Task<string> GenerateBuyerConfirmationCode()
        {
            try
            {
                string newref = $"BCF{DateTime.Now:yyyyMM}-{"1".ToString().PadLeft(4, '0')}";
                var result = await _db.LoadSingleAsync<string, dynamic>("spBuyerConfirmation_GenerateCode", new { });

                if (result != null)
                {
                    newref = $"BCF{DateTime.Now:yyyyMM}-{(Convert.ToInt32(result.Remove(0, result.Length - 4)) + 1).ToString().PadLeft(4, '0')}";
                }

                return newref;
            }
            catch (Exception) { throw; }
        }
    }
}