using DMS.Application.Interfaces.Setup.EmailLogRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.EmailLogDto;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using DMS.Domain.Enums;

namespace DMS.Web.Controllers.Setup
{
    [Authorize]
    public class EmailLogController : Controller
    {
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRoleAccessRepository _roleAccessRepo;
        

        public EmailLogController(
            IEmailLogRepository emailLogRepository,
            IBackgroundJobClient backgroundJobClient,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment,
            IRoleAccessRepository roleAccessRepo
            )
        {
            _emailLogRepository = emailLogRepository;
            _backgroundJobClient = backgroundJobClient;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
            _roleAccessRepo = roleAccessRepo;
             
        }

        #region Views

        public async Task<IActionResult> Index()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_EMSTUP);

                if (!roleAccess.CanRead)
                {
                    return View("AccessDenied");
                }

                ViewData["RoleAccess"] = roleAccess;

                var model = new EmailLogModel();
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Views

        #region API's

        public async Task<IActionResult> GetEmailById(int Id)
        {
            try
            {
                var data = await _emailLogRepository.GetById(Id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEmailLogs(EmailLogModel mailmodel)
        {
            try
            {
                int companyId = int.Parse(User.FindFirstValue("Company"));
                int userId = int.Parse(User.Identity.Name);
                mailmodel.Date = DateTime.Now;
                await _emailLogRepository.SaveEmailAsync(mailmodel, userId);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetEmailList() =>
          Ok(await _emailLogRepository.GetEmailLogList());

        #endregion API's
    }
}