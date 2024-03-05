using AutoMapper;
using DMS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Domain.Common;
using DMS.Domain.Dto.ModuleDto;
using DMS.Domain.Entities;
using DMS.Infrastructure.Persistence;
using DMS.Web.Controllers.Services;
using DMS.Web.Models;

namespace DMS.Web.Controllers.Setup;

[Authorize]
public class ModuleController : Controller
{
    private readonly IModuleRepository _moduleRepo;
    private readonly MNLTemplateDBContext _context;

    public ModuleController(IModuleRepository moduleRepo, MNLTemplateDBContext context)
    {
        _moduleRepo = moduleRepo;
        _context = context;
    }
    [ModuleServices(ModuleCodes.Module, typeof(IModuleRepository))]
    public IActionResult Index()
    {
        return View();
    }
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
    public async Task<IActionResult> SaveModule(ModuleViewModel model)
    {
        try
        {
            if (model.Module.Id != 0)
            {
                var Modules = await _moduleRepo.Module_GetAllModuleList();
                var Module = Modules.FirstOrDefault(x => x.Id == model.Module.Id);
                Module.ParentModuleId = model.Module.ParentModuleId;
                Module.Controller = model.Module.Controller;
                Module.Action = model.Module.Action;
                Module.Code = model.Module.Code;
                Module.Description = model.Module.Description;
                Module.BreadName = model.Module.BreadName;
                Module.Ordinal = model.Module.Ordinal;
                Module.ModuleStatusId = model.Module.ModuleStatusId;
                Module.IsBreaded = model.Module.IsBreaded;
                Module.IsVisible = model.Module.IsVisible;
                Module.Icon = model.Module.Icon;
                await _moduleRepo.SaveAsync(Module);
                return Ok("updated");
            }
            else
            {
                await _moduleRepo.SaveAsync(model.Module);
                return Ok("added");
            }
        }
        catch (Exception ex)
        {

            return BadRequest(ex);
        }
    }

    [HttpDelete]
    public async Task DeleteModules(int[] ids) =>
        await _moduleRepo.BatchDeleteAsync(ids);
}