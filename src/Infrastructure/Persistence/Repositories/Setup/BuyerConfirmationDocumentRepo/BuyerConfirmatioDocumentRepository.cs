using AutoMapper;
using DMS.Application.Interfaces.Setup.ApprovalStatusRepo;
using DMS.Application.Interfaces.Setup.BuyerConfirmationDocumentRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.BuyerConfirmationDocumentDto;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.BuyerConfirmationDocumentRepo
{
    public class BuyerConfirmationDocumentRepository : IBuyerConfirmationDocumentRepository
    {
        #region Fields

        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<BuyerConfirmationDocument> _contextHelper;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;
        private readonly IApprovalStatusRepository _approvalStatusRepo;

        public BuyerConfirmationDocumentRepository(
            DMSDBContext context,
            IMapper mapper,
            ISQLDatabaseService db,
            IApprovalStatusRepository approvalStatusRepo)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<BuyerConfirmationDocument>(context);
            _mapper = mapper;
            _db = db;
            _approvalStatusRepo = approvalStatusRepo;
        }

        #endregion Fields

        #region Operation Methods

        public async Task<BuyerConfirmationDocument> SaveAsync(BuyerConfirmationDocumentModel bcModel, int userId)
        {
            var buyerConfirm = _mapper.Map<BuyerConfirmationDocument>(bcModel);

            if (buyerConfirm.Id == 0)
            {
                if (buyerConfirm.Status is null)
                {
                    buyerConfirm.Status = (int)AppStatusType.Submitted;
                }

                int? intValue = buyerConfirm.Status;
                AppStatusType enumValue = (AppStatusType)intValue;

                buyerConfirm = await CreateAsync(buyerConfirm, userId);

                // Create Initial Approval Status
                await _approvalStatusRepo.CreateInitialApprovalStatusAsync(buyerConfirm.Id, ModuleCodes2.CONST_BCFUPLOAD, userId, bcModel.CompanyId.Value, enumValue);
            }
            else
            {
                buyerConfirm = await UpdateAsync(buyerConfirm, userId);
            }

            return buyerConfirm;
        }

        public async Task<BuyerConfirmationDocument> CreateAsync(BuyerConfirmationDocument buyerConfirm, int userId)
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

        public async Task<BuyerConfirmationDocument> UpdateAsync(BuyerConfirmationDocument buyerConfirm, int userId)
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

        public async Task DeleteAsync(BuyerConfirmationDocument buyerConfirm)
        {
            await _contextHelper.DeleteAsync(buyerConfirm);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.BuyerConfirmationDocuments.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.BuyerConfirmationDocuments.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }

        #endregion Operation Methods
    }
}