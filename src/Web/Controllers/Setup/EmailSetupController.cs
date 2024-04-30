using DMS.Application.Interfaces.Setup.EmailSetupRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.EmailSetupDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using DMS.Domain.Enums;
using DMS.Application.Interfaces.Setup.RoleRepository;

namespace DMS.Web.Controllers.Setup
{
    [Authorize]
    public class EmailSetupController : Controller
    {
         
    
        private readonly IEmailSetupRepository _emailSetupRepo;

        private readonly IRoleAccessRepository _roleAccessRepo;

        public EmailSetupController(ICurrentUserService currentUserService,
            IEmailSetupRepository emailSetupRepo, IRoleAccessRepository roleAccessRepo)
        {
           _roleAccessRepo = roleAccessRepo;
            _emailSetupRepo = emailSetupRepo;
        }
        #region Views

        public async Task<IActionResult> Index()
        {
            try
            {
                var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_EMSTUP);

                if (!roleAccess.CanRead)
                {
                    return View("AccessDenied");
                }

                ViewData["RoleAccess"] = roleAccess;

                var model = new EmailSetupModel();

                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region OPERATIONS

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(EmailSetupModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
                }
                int userid = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));
                model.CompanyId = companyId;
                await _emailSetupRepo.SaveAsync(model, userid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region API'S
        public async Task<IActionResult> GetList()
        {
            try
            {
                int companyId = int.Parse(User.FindFirstValue("Company"));
                return Ok(await _emailSetupRepo.GetList(companyId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion
    }
}
