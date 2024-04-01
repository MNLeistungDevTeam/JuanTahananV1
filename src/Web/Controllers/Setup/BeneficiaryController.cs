using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

public class BeneficiaryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}