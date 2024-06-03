using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

public class PropertyLocationController : Controller
{
    #region Fields

    private readonly IPropertyLocationRepository _propertyLocationRepo;

    public PropertyLocationController(IPropertyLocationRepository propertyLocationRepo)
    {
        _propertyLocationRepo = propertyLocationRepo;
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

    public async Task<IActionResult> GetAllPropertyLocation() =>
        Ok(await _propertyLocationRepo.GetAll());

    public async Task<IActionResult> GetPropertyLocationById(int id) =>
        Ok(await _propertyLocationRepo.GetById(id));

    #endregion API GETTERS

    #region API Actions

    [HttpPost]
    public async Task<IActionResult> SavePropertyLocation(PropertyManagementViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var userId = int.Parse(User.Identity.Name);

            var result = await _propertyLocationRepo.SaveAsync(model.PropLocModel, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePropertyLocation(string ids)
    {
        try
        {
            int[] Ids = Array.ConvertAll(ids.Split(','), int.Parse);

            await _propertyLocationRepo.BatchDeleteAsync(Ids);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion API Actions
}