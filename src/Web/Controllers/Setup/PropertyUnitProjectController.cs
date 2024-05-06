using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

public class PropertyUnitProjectController : Controller
{
    #region Fields

    private readonly IPropertyUnitProjectRepository _propertyUnitProjectRepo;

    public PropertyUnitProjectController(IPropertyUnitProjectRepository propertyUnitProjectRepo)
    {
        _propertyUnitProjectRepo = propertyUnitProjectRepo;
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
        Ok(await _propertyUnitProjectRepo.GetAll());

    public async Task<IActionResult> GetPropertyProjectLocationById(int id) =>
        Ok(await _propertyUnitProjectRepo.GetById(id));

    #endregion API GETTERS

    #region API Actions

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePropertyUnitProject(PropertyUnitProjectModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var userId = int.Parse(User.Identity.Name);

            var result = await _propertyUnitProjectRepo.SaveAsync(model, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("[controller]/DeletePropertyProjectLocation/{propertyProjectIds}")]
    public async Task<IActionResult> DeletePropertyUnitProject(string propertyUnitProjectIds)
    {
        try
        {
            int[] Ids = Array.ConvertAll(propertyUnitProjectIds.Split(','), int.Parse);

            await _propertyUnitProjectRepo.BatchDeleteAsync(Ids);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion API Actions
}