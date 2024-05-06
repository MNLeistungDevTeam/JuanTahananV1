using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace DMS.Web.Controllers.Setup;

public class PropertyUnitController : Controller
{
    #region Fields

    private readonly IPropertyUnitRepository _propertyUnitRepo;

    public PropertyUnitController(IPropertyUnitRepository propertyUnitRepo)
    {
        _propertyUnitRepo = propertyUnitRepo;
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

    public async Task<IActionResult> GetAllPropertyUnit() =>
        Ok(await _propertyUnitRepo.GetAll());

    public async Task<IActionResult> GetPropertyUnit(int id) =>
        Ok(await _propertyUnitRepo.GetById(id));

    #endregion API GETTERS

    #region API Actions

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePropertyUnit(PropertyUnitModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var userId = int.Parse(User.Identity.Name);

            var result = await _propertyUnitRepo.SaveAsync(model, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("[controller]/DeletePropertyUnit/{propertyProjectIds}")]
    public async Task<IActionResult> DeletePropertyUnit(string propertyUnitIds)
    {
        try
        {
            int[] Ids = Array.ConvertAll(propertyUnitIds.Split(','), int.Parse);

            await _propertyUnitRepo.BatchDeleteAsync(Ids);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion API Actions
}