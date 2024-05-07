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
    private readonly IPropertyLocationRepository _propertyLocationRepo;
    private readonly IPropertyProjectLocationRepository _propertyProjectLocationRepo;

    public PropertyProjectController(
        IPropertyProjectRepository propertyProjectRepo,
        IPropertyLocationRepository propertyLocationRepo,
        IPropertyProjectLocationRepository propertyProjectLocationRepo)
    {
        _propertyProjectRepo = propertyProjectRepo;
        _propertyLocationRepo = propertyLocationRepo;
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

    public async Task<IActionResult> GetAllPropertyProject() =>
        Ok(await _propertyProjectRepo.GetAllAsync());



    



    public async Task<IActionResult> GetPropertyProjectById(int id) =>
        Ok(await _propertyProjectRepo.GetById(id));

    public async Task<IActionResult> GetProjectLocationByProjectId(int id)
    {
        try
        {
            var projectLocation = await _propertyProjectLocationRepo.GetbyProjectId(id);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion API GETTERS

    #region API Actions

    [HttpPost]
    public async Task<IActionResult> SavePropertyProject(PropertyManagementViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var userId = int.Parse(User.Identity.Name);

            var result = await _propertyProjectRepo.SaveAsync(model.PropProjModel, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePropertyProject(string ids)
    {
        try
        {
            int[] Ids = Array.ConvertAll(ids.Split(','), int.Parse);

            await _propertyProjectRepo.BatchDeleteAsync(Ids);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion API Actions
}