using AutoMapper;
using DMS.Application.Interfaces.Setup.ApprovalStatusRepo;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.BuyerConfirmationRepo;

public class BuyerConfirmationRepository : IBuyerConfirmationRepository
{
    #region Fields

    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<BuyerConfirmation> _contextHelper;
    private readonly IMapper _mapper;
    private readonly ISQLDatabaseService _db;
    private readonly IApprovalStatusRepository _approvalStatusRepo;

    public BuyerConfirmationRepository(
        DMSDBContext context,
        IMapper mapper,
        ISQLDatabaseService db,
        IApprovalStatusRepository approvalStatusRepo)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<BuyerConfirmation>(context);
        _mapper = mapper;
        _db = db;
        _approvalStatusRepo = approvalStatusRepo;
    }

    #endregion Fields

    #region Getters

    public async Task<BuyerConfirmation?> GetByIdAsync(int id) =>
      await _context.BuyerConfirmations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<BuyerConfirmationModel?> GetAsync(int id) =>
        await _db.LoadSingleAsync<BuyerConfirmationModel, dynamic>("spBuyerConfirmation_Get", new { id });

    public async Task<IEnumerable<BuyerConfirmationModel>> GetAllAsync() =>
        await _db.LoadDataAsync<BuyerConfirmationModel, dynamic>("spBuyerConfirmation_GetAll", new { });

    public async Task<BuyerConfirmationModel?> GetByUserAsync(int userId) =>
        await _db.LoadSingleAsync<BuyerConfirmationModel, dynamic>("spBuyerConfirmation_GetByUserId", new { userId });

    public async Task<BuyerConfirmationModel?> GetByCodeAsync(string code) =>
        await _db.LoadSingleAsync<BuyerConfirmationModel, dynamic>("spBuyerConfirmation_GetByCode", new { code });

    public async Task<BuyerConfirmationInqModel?> GetInqAsync(int companyId) =>
        await _db.LoadSingleAsync<BuyerConfirmationInqModel, dynamic>("spBuyerConfirmation_GetInq", new { companyId });

    #endregion Getters

    #region Operation Methods

    public async Task<BuyerConfirmation> SaveAsync(BuyerConfirmationModel bcModel, int userId)
    {
        var buyerConfirm = _mapper.Map<BuyerConfirmation>(bcModel);

        if (buyerConfirm.Id == 0)
        {
            if (buyerConfirm.ApprovalStatus is null)
            {
                buyerConfirm.ApprovalStatus = (int)AppStatusType.Draft;
            }

            buyerConfirm.Code = await GenerateBuyerConfirmationCode();

            int? intValue = buyerConfirm.ApprovalStatus;
            AppStatusType enumValue = (AppStatusType)intValue;

            buyerConfirm = await CreateAsync(buyerConfirm, userId);

            // Create Initial Approval Status
            await _approvalStatusRepo.CreateInitialApprovalStatusAsync(buyerConfirm.Id, ModuleCodes2.CONST_BCFREQUESTS, userId, bcModel.CompanyId.Value, enumValue);
        }
        else
        {
            

            buyerConfirm = await UpdateAsync(buyerConfirm, userId);
        }

        return buyerConfirm;
    }

    public async Task<BuyerConfirmation> CreateAsync(BuyerConfirmation buyerConfirm, int userId)
    {
        try
        {
            buyerConfirm.CreatedById = userId;
            buyerConfirm.DateCreated = DateTime.Now;

            var result = await _contextHelper.CreateAsync(buyerConfirm, "DateModified", "ModifiedById");

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<BuyerConfirmation> UpdateAsync(BuyerConfirmation buyerConfirm, int userId)
    {
        try
        {
            buyerConfirm.ModifiedById = userId;
            buyerConfirm.DateModified = DateTime.Now;

            var result = await _contextHelper.UpdateAsync(buyerConfirm, "DateCreated", "CreatedById");
            return result;
        }
        catch (Exception)
        {
            throw;
        }
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

    #endregion Operation Methods

    #region Helpers Methods

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

    #endregion Helpers Methods
}