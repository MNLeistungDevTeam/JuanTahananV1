using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

public class PropertyProjectController : Controller
{
    #region Fields

    private readonly IPropertyProjectRepository _propertyProjectRepo;

    public PropertyProjectController(IPropertyProjectRepository propertyProjectRepo)
    {
        _propertyProjectRepo = propertyProjectRepo;
    }

    #endregion Fields

    #region Views

    public IActionResult Index()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); ;
        }
    }

    #endregion Views

    #region API GETTERS

    public async Task<IActionResult> GetAllPropertyProject() =>
        Ok(await _propertyProjectRepo.GetAll());

    public async Task<IActionResult> GetPropertyProjectById(int id) =>
        Ok(await _propertyProjectRepo.GetById(id));

    #endregion API GETTERS

    #region API Actions

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePropertyProject(PropertyProjectModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var userId = int.Parse(User.Identity.Name);

            var result = await _propertyProjectRepo.SaveAsync(model, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("[controller]/DeletePropertyProject/{propertyProjectIds}")]
    public async Task<IActionResult> DeletePropertyProject(string propertyProjectIds)
    {
        try
        {
            int[] Ids = Array.ConvertAll(propertyProjectIds.Split(','), int.Parse);

            await _propertyProjectRepo.BatchDeleteAsync(Ids);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion API Actions
}