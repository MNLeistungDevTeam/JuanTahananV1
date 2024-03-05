using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DMS.Application.Services;

namespace DMS.Web.Controllers;

[Authorize]
public class LongOperationController : Controller
{
    private readonly ILongOperationSignalRService _longOperationService;

    public LongOperationController(ILongOperationSignalRService longOperationService)
    {
        _longOperationService = longOperationService;
    }

    public async Task<IActionResult> StartLongOperation()
    {
        await _longOperationService.RunLongOperation();
        return Ok();
    }
}
