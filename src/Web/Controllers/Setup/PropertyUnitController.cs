using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Services;
using DMS.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

public class PropertyUnitController : Controller
{
    #region Fields

    private readonly IPropertyUnitRepository _propertyUnitRepo;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileUploadService _fileUploadService;

    public PropertyUnitController(IPropertyUnitRepository propertyUnitRepo, IWebHostEnvironment webHostEnvironment, IFileUploadService fileUploadService)
    {
        _propertyUnitRepo = propertyUnitRepo;
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

    public async Task<IActionResult> GetAllPropertyUnit() =>
        Ok(await _propertyUnitRepo.GetAll());

    public async Task<IActionResult> GetPropertyUnit(int id) =>
        Ok(await _propertyUnitRepo.GetById(id));

    #endregion API GETTERS

    #region API Actions

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePropertyUnit(PropertyManagementViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var userId = int.Parse(User.Identity.Name);


            var rootFolder = _webHostEnvironment.WebRootPath;
            string profileSaveLocation = Path.Combine("Files", "Images", "PropertyManagementFile", "PropertyUnit", model.PropUnitModel.Name);
            var unitProfileImage = await _fileUploadService.SaveFileAsync(model.PropUnitModel?.ProfileImageFile, profileSaveLocation, rootFolder);



            model.PropUnitModel.ProfileImage = unitProfileImage;

            var result = await _propertyUnitRepo.SaveAsync(model.PropUnitModel, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePropertyUnit(string ids)
    {
        try
        {
            int[] Ids = Array.ConvertAll(ids.Split(','), int.Parse);

            await _propertyUnitRepo.BatchDeleteAsync(Ids);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion API Actions
}