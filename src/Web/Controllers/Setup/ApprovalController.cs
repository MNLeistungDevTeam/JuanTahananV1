using DMS.Application.Interfaces.Setup.ApprovalStatusRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.ModuleStageApproverRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.ApprovalLevelDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup
{
    public class ApprovalController : Controller
    {
        private readonly IModuleRepository _moduleRepo;
        private readonly IModuleStageApproverRepository _moduleStageApproverRepo;
        private readonly IApprovalService _approvalService;
        private readonly IApprovalStatusRepository _approvalStatusRepo;

        public ApprovalController(IModuleRepository moduleRepo, IModuleStageApproverRepository moduleStageApproverRepo, IApprovalService approvalService, IApprovalStatusRepository approvalStatusRepo)
        {
            _moduleRepo = moduleRepo;
            _moduleStageApproverRepo = moduleStageApproverRepo;
            _approvalService = approvalService;
            _approvalStatusRepo = approvalStatusRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("[controller]/GetByReferenceModuleCodeAsync/{referenceId}/{moduleCode?}")]
        public async Task<IActionResult> GetByReferenceModuleCodeAsync(int referenceId, string? moduleCode)
        {
            int companyId = int.Parse(User.FindFirstValue("Company"));

            var data = await _approvalStatusRepo.GetByReferenceModuleCodeAsync(referenceId, moduleCode, companyId);

            return Ok(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveModuleStageApprover(ModuleViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
                    return Conflict(errors);
                }

                var userId = int.Parse(User.Identity.Name);

                await _moduleStageApproverRepo.SaveModuleStageApprover(model.Module.Id, model.ModuleStages, userId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveApprovalLevel(ApprovalLevelViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
                }

                var userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));

                var approvalLevel = new ApprovalLevelModel
                {
                    Id = model.ApprovalLevel.Id,
                    ApprovalStatusId = model.ApprovalLevel.ApprovalStatusId,
                    Status = model.ApprovalLevel.Status,
                    Remarks = model.ApprovalLevel.Remarks,
                    ApproverId = userId,
                    DateUpdated = DateTime.Now
                };
                await _approvalService.SaveApprovalLevel(approvalLevel, userId, companyId);

                //await _notificationService.NotifyUsersByApproval(model.ApprovalLevel.ApprovalStatusId, userId, companyId);

                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}