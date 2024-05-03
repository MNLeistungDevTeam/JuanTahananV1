using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers.Setup
{
    public class BuyerConfirmationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
