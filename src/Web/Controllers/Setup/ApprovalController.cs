using DMS.Application.Interfaces.Setup.ModuleRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup
{
    public class ApprovalController : Controller
    {
        private readonly IModuleRepository _moduleRepo;

        public ApprovalController(IModuleRepository moduleRepo)
        {
            _moduleRepo = moduleRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}