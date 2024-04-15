using DMS.Application.Interfaces.Setup.AddressRepo;
using DMS.Application.Interfaces.Setup.CompanyLogoRepo;
using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.CompanySettingRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

[Authorize]
public class CompanyController : Controller
{
    #region Fields

    private readonly ICompanyRepository _companyRepo;
    private readonly ICompanySettingRepository _companySettingsRepo;
    private readonly ICompanyLogoRepository _companyLogoRepo;
    private readonly IAddressRepository _addressRepo;
    private readonly IFileUploadService _fileUploadService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IRoleAccessRepository _roleAccessRepo;

    public CompanyController(
        ICompanyRepository companyRepo,
        ICompanySettingRepository companySettingsRepo,
        ICompanyLogoRepository companyLogoRepo,
        IFileUploadService fileUploadService,
        IWebHostEnvironment webHostEnvironment,
        IAddressRepository addressRepo,
        IRoleAccessRepository roleAccessRepo)
    {
        _companyRepo = companyRepo;
        _companySettingsRepo = companySettingsRepo;
        _companyLogoRepo = companyLogoRepo;
        _fileUploadService = fileUploadService;
        _webHostEnvironment = webHostEnvironment;
        _addressRepo = addressRepo;

        _roleAccessRepo = roleAccessRepo;
    }

    #endregion Fields

    #region Views

    public async Task<IActionResult> Index()
    {
        try
        {
            var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_COMP);

            if (roleAccess is null) { return View("AccessDenied"); }
            if (!roleAccess.CanRead) { return View("AccessDenied"); }

            ViewData["RoleAccess"] = roleAccess;

            var companyVm = new CompanyViewModel();
            var companyLogos = new List<CompanyLogoModel>
        {
            new CompanyLogoModel { Description = string.Empty },
            new CompanyLogoModel { Description = string.Empty }
        };

            companyVm.CompanyLogo = companyLogos;

            return View(companyVm);
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }

    public async Task<IActionResult> Create()
    {
        try
        {
            var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_COMP);

            if (roleAccess is null) { return View("AccessDenied"); }
            if (!roleAccess.CanCreate) { return View("AccessDenied"); }

            var companyVm = new CompanyViewModel
            {
                Company = new(),
                CompanyLogo = new(),
                Address = new()
            };

            return View("Create", companyVm);
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }

    [Route("CompanyProfile")]
    public async Task<IActionResult> CompanyProfile()
    {
        try
        {
            var companyId = int.Parse(User.FindFirstValue("Company"));
            var company = await _companyRepo.GetCompany(companyId);
            var companyLogo = await _companyLogoRepo.GetByCompanyId(companyId);
            var companySettings = new CompanySettingModel();

            var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_COMP);

            if (roleAccess is null) { return View("AccessDenied"); }
            if (!roleAccess.CanRead) { return View("AccessDenied"); }

            ViewData["RoleAccess"] = roleAccess;

            var viewModel = new CompanyViewModel
            {
                Company = company,
                CompanyLogo = companyLogo.ToList(),
                Setting = companySettings
            };

            return View(viewModel);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion Views

    #region API Getters

    //Company Repository
    public async Task<IActionResult> GetSubCompanies(int id)
    {
        try
        {
            var data = await _companyRepo.GetCompanies();

            return Ok(data.Where(m => m.Id != id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> GetCompanies() =>
       Ok(await _companyRepo.GetCompanies());

    public async Task<IActionResult> GetCompany(int id)
    {
        var data = await _companyRepo.GetCompany(id);

        return Ok(data);
    }

    public async Task<IActionResult> GetCompanyInfo(int id)
    {
        var data = await _companyRepo.GetCompanyInfo(id);

        return Ok(data);
    }

    public async Task<IActionResult> GetLoggedInCompany()
    {
        int companyId = int.Parse(User.FindFirstValue("Company"));
        var data = await _companyRepo.GetCompany(companyId);

        return Ok(data);
    }

    //Company Logo Repository
    public async Task<IActionResult> GetCompanyLogos(int companyId)
    {
        var data = await _companyLogoRepo.GetByCompanyId(companyId);

        return Ok(data);
    }

    //Address Repository
    public async Task<IActionResult> GetCompanyAddresses(int companyId)
    {
        //int compId = int.Parse(User.FindFirstValue("Company"));
        //var data = await _addressRepo.GetByReferenceId(compId, (int)AddressReferenceType.Company);
        var data = await _addressRepo.GetByReferenceId(companyId, (int)AddressReferenceType.Index.Company);

        return Ok(data);
    }

    //Company Settings Repository
    public async Task<IActionResult> GetCompanySettingByCompanyId(int companyId)
    {
        var data = await _companySettingsRepo.GetByCompanyIdAsync(companyId);

        return Ok(data);
    }

    [AllowAnonymous]
    public async Task<IActionResult> GetCompanyLogoByDesc(string description, int? companyId = 0)
    {
        try
        {
            if (!companyId.HasValue) { companyId = int.Parse(User.FindFirstValue("Company")); }
            if (companyId == 0) { companyId = int.Parse(User.FindFirstValue("Company")); }

            var result = await _companyLogoRepo.GetByDesc(companyId ?? 0, description);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion API Getters

    #region Operation Methods

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCompany(CompanyViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
            }

            var userId = int.Parse(User.Identity.Name);
            var rootFolder = _webHostEnvironment.WebRootPath;

            if (model.CompanyLogo != null && model.CompanyLogo.Count > 0)
            {
                foreach (var item in model.CompanyLogo)
                {
                    var saveLocation = Path.Combine("Files", "Images", "CompanyLogoFile", model.Company.Name ?? string.Empty);
                    var location = await _fileUploadService.SaveFileAsync(item.CompanyLogoFile, saveLocation, rootFolder);

                    item.Location = location ?? string.Empty;
                }
            }

            await _companyRepo.SaveAsync(model.Company, model.CompanyLogo, model.Address, model.Setting, userId, _webHostEnvironment.WebRootPath);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("Company/DeleteCompanies/")]
    public async Task<IActionResult> DeleteCompanies(string companyIds)
    {
        try
        {
            int[] ids = Array.ConvertAll(companyIds.Split(','), int.Parse);

            await _companyRepo.BatchDeleteAsync(ids);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion Operation Methods

}