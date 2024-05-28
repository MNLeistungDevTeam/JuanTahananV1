using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

public class PropertyProjectController : Controller
{
    #region Fields

    private readonly IPropertyProjectRepository _propertyProjectRepo;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileUploadService _fileUploadService;

    public PropertyProjectController(IPropertyProjectRepository propertyProjectRepo, IWebHostEnvironment webHostEnvironment, IFileUploadService fileUploadService)
    {
        _propertyProjectRepo = propertyProjectRepo;
        _webHostEnvironment = webHostEnvironment;
        _fileUploadService = fileUploadService;
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

    public async Task<IActionResult> GetPropertyLocationByProject(int id) =>
       Ok(await _propertyProjectRepo.GetPropertyLocationByProjectAsync(id));

    public async Task<IActionResult> GetPropertyUnitByProject(int id) =>
        Ok(await _propertyProjectRepo.GetPropertyUnitByProjectAsync(id));

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

            var rootFolder = _webHostEnvironment.WebRootPath;

            string projectName = model.PropProjModel.Name.Trim();
            string profileSaveLocation = Path.Combine("Files", "Images", "PropertyManagementFile", "PropertyProject", projectName);

            var projectProfileImage = await _fileUploadService.SaveFileAsync(model.PropProjModel?.ProfileImageFile, profileSaveLocation, rootFolder);

            model.PropProjModel.ProfileImage = projectProfileImage;

            var result = await _propertyProjectRepo.SaveAsync(model.PropProjModel, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProjectLocation(PropertyManagementViewModel vwModel)
    {

        //if (!ModelState.IsValid)
        //    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

        try
        {
            var userId = int.Parse(User.Identity.Name);
            var companyId = int.Parse(User.FindFirstValue("Company"));

            List<PropertyProjectLocationModel> dcModel = new();

            if (vwModel.PropertyLocationIds != null)
            {
                string[] tempIds = vwModel.PropertyLocationIds.Split(',');
                int[] coaIds = Array.ConvertAll(tempIds, s => int.Parse(s));

                foreach (var item in coaIds)
                {
                    PropertyProjectLocationModel dCoa = new()
                    {
                        ProjectId = vwModel.PropProjLocModel.ProjectId,
                        LocationId = item
                    };

                    dcModel.Add(dCoa);
                }
            }

            await _propertyProjectRepo.SaveProjectLocations(vwModel.PropProjModel, dcModel, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProjectUnit(PropertyManagementViewModel vwModel)
    {
        try
        {
            //if (!ModelState.IsValid)
            //    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var userId = int.Parse(User.Identity.Name);
            var companyId = int.Parse(User.FindFirstValue("Company"));

            List<PropertyUnitProjectModel> dcModel = new();

            if (vwModel.PropertyUnitIds != null)
            {
                string[] tempIds = vwModel.PropertyUnitIds.Split(',');
                int[] coaIds = Array.ConvertAll(tempIds, s => int.Parse(s));

                foreach (var item in coaIds)
                {
                    PropertyUnitProjectModel dCoa = new()
                    {
                        ProjectId = vwModel.PropUnitProjModel.ProjectId,
                        UnitId = item
                    };

                    dcModel.Add(dCoa);
                }
            }

            await _propertyProjectRepo.SaveProjectUnits(vwModel.PropProjModel, dcModel, userId);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
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