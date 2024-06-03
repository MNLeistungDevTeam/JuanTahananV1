using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers.Setup
{
    public class TempLinkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
