using DMS.Application.Interfaces.Setup.ApprovalStatusRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.ModuleStageApproverRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApprovalLevelDto;
using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IRoleAccessRepository _roleAccessRepo;

        public ApprovalController(IModuleRepository moduleRepo,
            IModuleStageApproverRepository moduleStageApproverRepo,
            IApprovalService approvalService,
            IApprovalStatusRepository approvalStatusRepo,
            IRoleAccessRepository roleAccessRepo)
        {
            _moduleRepo = moduleRepo;
            _moduleStageApproverRepo = moduleStageApproverRepo;
            _approvalService = approvalService;
            _approvalStatusRepo = approvalStatusRepo;
            _roleAccessRepo = roleAccessRepo;
        }

        #region View

        public async Task<IActionResult> Index()
        {
            try
            {
                var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_APPROVERMNGMNT);

                if (roleAccess is null) { return View("AccessDenied"); }
                if (!roleAccess.CanRead) { return View("AccessDenied"); }

                ViewData["RoleAccess"] = roleAccess;

                return View();
            }
            catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
        }

        #endregion View

        #region Api

        #region Get

        [Route("[controller]/GetByReferenceModuleCodeAsync/{referenceId}/{moduleCode?}")]
        public async Task<IActionResult> GetByReferenceModuleCodeAsync(int referenceId, string? moduleCode)
        {
            int companyId = int.Parse(User.FindFirstValue("Company"));

            //var module = await _moduleRepo.GetByCodeAsync(moduleCode);
            var data = await _approvalStatusRepo.GetByReferenceIdAsync(referenceId, companyId);

            return Ok(data);
        }

        #endregion Get

        #region Action Methods

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
        public async Task<IActionResult> SaveApprovalLevel(ApprovalLevelModel model)
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
                    Id = model.Id,
                    ApprovalStatusId = model.ApprovalStatusId,
                    Status = model.Status,
                    Remarks = model.Remarks,
                    ApproverId = userId,
                    DateUpdated = DateTime.Now
                };
                await _approvalService.SaveApprovalLevel(approvalLevel, userId, companyId);

                //await _notificationService.NotifyUsersByApproval(model.ApprovalLevel.ApprovalStatusId, userId, companyId);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        #endregion Action Methods

        #endregion Api
    }
}