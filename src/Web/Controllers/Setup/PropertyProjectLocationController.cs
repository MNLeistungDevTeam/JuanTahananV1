using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace DMS.Web.Controllers.Setup;

public class PropertyProjectLocationController : Controller
{
    #region Fields

    private readonly IPropertyProjectLocationRepository _propertyProjectLocationRepo;

    public PropertyProjectLocationController(IPropertyProjectLocationRepository propertyProjectLocationRepo)
    {
        _propertyProjectLocationRepo = propertyProjectLocationRepo;
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

    public async Task<IActionResult> GetAllPropertyProjectLocation() =>
        Ok(await _propertyProjectLocationRepo.GetAll());

    public async Task<IActionResult> GetPropertyProjectLocationById(int id) =>
        Ok(await _propertyProjectLocationRepo.GetById(id));

    #endregion API GETTERS

    #region API Actions

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePropertyProjectLocation(PropertyProjectLocationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var userId = int.Parse(User.Identity.Name);

            var result = await _propertyProjectLocationRepo.SaveAsync(model, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("[controller]/DeletePropertyProjectLocation/{propertyProjectIds}")]
    public async Task<IActionResult> DeletePropertyProjectLocation(string propertyProjectLocationIds)
    {
        try
        {
            int[] Ids = Array.ConvertAll(propertyProjectLocationIds.Split(','), int.Parse);

            await _propertyProjectLocationRepo.BatchDeleteAsync(Ids);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion API Actions
}