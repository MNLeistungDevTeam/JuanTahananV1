using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers.Authentication
{
    public class TempLinkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
