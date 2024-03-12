using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.ModuleStageApproverRepo;
using DMS.Application.Interfaces.Setup.ModuleStageRepo;
using DMS.Application.Interfaces.Setup.ModuleTypeRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Domain.Dto.ModuleDto;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Dto.ModuleTypeDto;
using DMS.Domain.Dto.OtherDto;
using DMS.Domain.Enums;
using DMS.Infrastructure.Persistence;
using DMS.Web.Controllers.Services;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

[Authorize]
public class ModuleController : Controller
{
    private readonly IModuleRepository _moduleRepo;
    private readonly DMSDBContext _context;
    private readonly IModuleTypeRepository _moduleTypeRepo;
    private readonly IModuleStageRepository _moduleStageRepo;
    private readonly IModuleStageApproverRepository _moduleStageApproverRepo;
    private readonly IRoleAccessRepository _currentUserRoleAccessService;

    public ModuleController(IModuleRepository moduleRepo,
        DMSDBContext context,
        IModuleTypeRepository moduleTypeRepo,
        IModuleStageRepository moduleStageRepo,
        IModuleStageApproverRepository moduleStageApproverRepo,
        IRoleAccessRepository currentUserRoleAccessService)
    {
        _moduleRepo = moduleRepo;
        _context = context;
        _moduleTypeRepo = moduleTypeRepo;
        _moduleStageRepo = moduleStageRepo;
        _moduleStageApproverRepo = moduleStageApproverRepo;
        _currentUserRoleAccessService = currentUserRoleAccessService;
    }

    //[ModuleServices(ModuleCodes.Module, typeof(IModuleRepository))]

    public async Task<IActionResult> Index()
    {
        try
        {
            var roleAccess = await _currentUserRoleAccessService.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_MODULE);

            if (roleAccess == null) { return View("AccessDenied"); }
            if (!roleAccess.CanRead) { return View("AccessDenied"); }

            var approvalRouteTypes = new List<DropdownModel>()
            {
                new() { Id = 1, Description = "Straight" },
                new() { Id = 2, Description = "Total Count" }
            };

            var module = new ModuleModel
            {
                ApprovalRouteTypes = approvalRouteTypes.ToList()
            };

            var moduleStages = new List<ModuleStageModel>
            {
                new() { Name = "Review" },
                new() { Name = "Approve" }
            };

            ViewData["RoleAccess"] = roleAccess;

            var viewModel = new ModuleViewModel
            {
                Module = module,
                ModuleStages = moduleStages
            };

            return View(viewModel);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Old Api

    public async Task<IActionResult> GetModuleStatus()
    {
        var modulesStatus = await _context.ModuleStatuses.ToListAsync();

        var ModulesStats = modulesStatus
            .Select(x => new
            {
                text = x.Description,
                value = x.Id
            });
        return Ok(ModulesStats);
    }

    public async Task<IActionResult> GetAllModules() =>
        Ok(await _moduleRepo.Module_GetAllModuleList());

    public async Task<IActionResult> GetModuleById(int id) =>
         Ok((await _moduleRepo.Module_GetAllModuleList()).FirstOrDefault(x => x.Id == id));

    public async Task<IActionResult> GetParentModule(int id)
    {
        var modules = await _moduleRepo.Module_GetAllModuleList();

        var parentModules = modules
            .Where(x => x.Id != id)
            .Select(x => new
            {
                text = x.Description,
                value = x.Id
            });

        return Ok(parentModules);
    }

    [HttpPost]
    [ModelStateValidations(typeof(ModuleViewModel))]
    //public async Task<IActionResult> SaveModule(ModuleViewModel model)
    //{
    //    try
    //    {
    //        if (model.Module.Id != 0)
    //        {
    //            var Modules = await _moduleRepo.Module_GetAllModuleList();
    //            var Module = Modules.FirstOrDefault(x => x.Id == model.Module.Id);
    //            Module.ParentModuleId = model.Module.ParentModuleId;
    //            Module.Controller = model.Module.Controller;
    //            Module.Action = model.Module.Action;
    //            Module.Code = model.Module.Code;
    //            Module.Description = model.Module.Description;
    //            Module.BreadName = model.Module.BreadName;
    //            Module.Ordinal = model.Module.Ordinal;
    //            Module.ModuleStatusId = model.Module.ModuleStatusId;
    //            Module.IsBreaded = model.Module.IsBreaded;
    //            Module.IsVisible = model.Module.IsVisible;
    //            Module.Icon = model.Module.Icon;
    //            await _moduleRepo.SaveAsync(Module);
    //            return Ok("updated");
    //        }
    //        else
    //        {
    //            await _moduleRepo.SaveAsync(model.Module);
    //            return Ok("added");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex);
    //    }
    //}

    [HttpDelete]
    public async Task DeleteModules(int[] ids) =>
        await _moduleRepo.BatchDeleteAsync(ids);

    #endregion Old Api

    #region Modules API

    public async Task<IActionResult> GetModules()
    {
        var data = await _moduleRepo.GetAllAsync();
        return Ok(data);
    }

    public async Task<IActionResult> GetParentModules(int moduleTypeId)
    {
        try
        {
            var modules = await _moduleRepo.GetAllAsync();
            var parentModulesDropDown = modules
                .Where(m => m.ModuleTypeId == moduleTypeId && (m.ParentModuleId == 0 || m.ParentModuleId == null))
                .Select(m => new DropdownModel { Id = m.Id, Description = m.Description })
                .ToList();
            return Ok(parentModulesDropDown);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> GetModule(int id)
    {
        var data = await _moduleRepo.GetByIdAsync(id);
        return Ok(data);
    }

    public async Task<IActionResult> GetModuleByCode(string code)
    {
        var data = await _moduleRepo.GetByCodeAsync(code);
        return Ok(data);
    }

    public async Task<IActionResult> GetModuleStages(int id)
    {
        var data = await _moduleStageRepo.GetByModuleId(id);
        return Ok(data);
    }

    public async Task<IActionResult> GetModuleTypes()
    {
        var data = await _moduleTypeRepo.GetAllAsync();
        return Ok(data);
    }

    public async Task<IActionResult> GetModuleType(int id)
    {
        var data = await _moduleTypeRepo.GetByIdAsync(id);
        return Ok(data);
    }

    public async Task<IActionResult> GetModuleByType(int typeId)
    {
        try
        {
            var data = await _moduleRepo.GetAllAsync();

            return Ok(data.Where(m => m.ModuleTypeId == typeId));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    public async Task<IActionResult> GetWithApprovers()
    {
        try
        {
            var data = await _moduleRepo.GetWithApproversAsync();

            return Ok(data);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveModuleType(ModuleTypeModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
                return Conflict(errors);
            }

            var userId = int.Parse(User.Identity.Name);

            await _moduleTypeRepo.SaveAsync(model, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("Module/DeleteModuleTypes/")]
    public async Task<IActionResult> DeleteModuleTypes(string moduleTypeIds)
    {
        try
        {
            int[] Ids = Array.ConvertAll(moduleTypeIds.Split(','), int.Parse);

            await _moduleStageRepo.BatchDeleteAsync(Ids);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> GetStageApprovers(int id)
    {
        try
        {
            var data = await _moduleStageApproverRepo.GetByModuleStageId(id);

            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> GetStageApprover(int id)
    {
        try
        {
            var data = await _moduleStageApproverRepo.GetByModuleStageId(id);

            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveModule(ModuleViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
                return Conflict(errors);
            }

            var userId = int.Parse(User.Identity.Name);

            await _moduleRepo.SaveAsync(model.Module, model.ModuleStages, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("Module/DeleteModules/")]
    public async Task<IActionResult> DeleteModules(string moduleIds)
    {
        try
        {
            int[] Ids = Array.ConvertAll(moduleIds.Split(','), int.Parse);

            if (Ids.Length > 0)
            {
                foreach (var moduleId in Ids)
                {
                    await _moduleRepo.DeleteAsync(moduleId);
                }
            }

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion Modules API
}