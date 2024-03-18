using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Threading.Tasks;
using DMS.Application.Interfaces.Setup.ModuleStageApproverRepo;

namespace DMS.Web.Controllers.Setup
{
    public class ApprovalController : Controller
    {
        private readonly IModuleRepository _moduleRepo;
        private readonly IModuleStageApproverRepository _moduleStageApproverRepo;

        public ApprovalController(IModuleRepository moduleRepo, IModuleStageApproverRepository moduleStageApproverRepo)
        {
            _moduleRepo = moduleRepo;
            _moduleStageApproverRepo = moduleStageApproverRepo;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}