using AutoMapper;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

[Authorize]
public class RoleController : Controller
{
    #region Fields

    private readonly IRoleRepository _roleRepo;
    private readonly IRoleAccessRepository _roleAccessRepo;
    private readonly IModuleRepository _moduleRepo;
    private readonly IMapper _mapper;
    private readonly IUserRoleRepository _userRoleRepo;
    private readonly IUserRepository _userRepo;

    public RoleController(
        IRoleRepository roleRepo,
        IRoleAccessRepository roleAccessRepo,
        IModuleRepository moduleRepo,
        IMapper mapper,
        IUserRoleRepository userRoleRepo,
        IUserRepository userRepo)
    {
        _roleRepo = roleRepo;
        _roleAccessRepo = roleAccessRepo;
        _moduleRepo = moduleRepo;
        _mapper = mapper;
        _userRoleRepo = userRoleRepo;
        _userRepo = userRepo;
    }

    #endregion Fields

    #region Views

    public async Task<IActionResult> Index()
    {
        var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_ROLE_MGMT);

        if (roleAccess is null) { return View("AccessDenied"); }
        if (!roleAccess.CanRead) { return View("AccessDenied"); }
        return View();
    }

    //public async Task<IActionResult> UserRole(int id)
    //{
    //    return View(new RoleViewModel()
    //    {
    //        Role = _mapper.Map<RoleModel>(await _roleRepo.GetByIdAsync(id))
    //    });
    //}

    [Route("Role/UserRole/{code}")]
    public async Task<IActionResult> UserRole(string code)
    {
        try
        {
            var result = await _roleRepo.GetByCodeAsync(code);

            RoleViewModel vModel = new()
            {
                Role = _mapper.Map<RoleModel>(result)
            };

            return View(vModel);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    #endregion Views

    #region API Getters

    public async Task<IActionResult> GetRoleSetupAccess(int id)
    {
        var lists = new List<HybridRoles>();
        var role = await _roleRepo.GetByIdAsync(id) ?? new Role();
        var items = (await _moduleRepo.Module_GetAllModuleList()).ToList();
        for (int i = 0; i < items.Count; ++i)
        {
            var item = items[i];
            var roleaccess = (await _roleAccessRepo.GetAllAsync()).FirstOrDefault(x => x.RoleId == role.Id && x.ModuleId == item.Id) ?? new RoleAccess()
            {
                Id = 0,
                RoleId = 0,
                CanCreate = false,
                CanDelete = false,
                CanRead = false,
                CanModify = false,
            };
            lists.Add(new HybridRoles(
                canModify: roleaccess.CanModify,
                index: i,
                id: roleaccess.Id,
                roleId: role.Id,
                moduleId: item.Id,
                canCreate: roleaccess.CanCreate,
                canDelete: roleaccess.CanDelete,
                canRead: roleaccess.CanRead,
                moduleName: item.Description
                ));
        }
        return Ok(lists);
    }

    public async Task<IActionResult> GetRoleById(int id) =>
        Ok(await _roleRepo.GetByIdAsync(id));

    public async Task<IActionResult> GetAllRoles() =>
        Ok(await _roleRepo.GetAllRolesAsync());

    public async Task<IActionResult> GetUsers(int roleId)
    {
        var userRoles = (await _userRoleRepo.GetAllAsync()).Where(x => x.RoleId == roleId).Select(s => s.UserId).ToList();

        List<UserModel> usersWithoutRole = (await _userRepo.GetUsersAsync()).ToList();

        if (userRoles.Any())
        {
            usersWithoutRole = usersWithoutRole.Where(x => !userRoles.Contains(x.Id)).ToList();
        }

        var result = usersWithoutRole.Select(x => new { text = x.Name, value = x.Id, position = x.Position });

        return Ok(result);
    }

    public async Task<IActionResult> GetUserRoles(int RoleId) =>
        Ok(((await _userRoleRepo.SpGetAllRoles())).Where(x => x.RoleId == RoleId));

    #endregion API Getters

    #region API Operation

    //[HttpPost]
    //public async Task<IActionResult> SaveUserRole(RoleViewModel model)
    //{
    //    try
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
    //        }

    //        foreach (var item in model.UserRole.UsersId)
    //        {
    //            var existingUserRole = await _userRoleRepo.GetUserRoleAsync(item);

    //            if (existingUserRole != null)
    //            {
    //                // Update existing user role if it already exists
    //                existingUserRole.RoleId = model.UserRole.RoleId;
    //                await _userRoleRepo.SaveAsync(_mapper.Map<UserRoleModel>(existingUserRole));
    //            }
    //            else
    //            {
    //                // Create new user role if it doesn't exist
    //                await _userRoleRepo.SaveAsync(new UserRoleModel()
    //                {
    //                    RoleId = model.UserRole.RoleId,
    //                    UserId = item
    //                });
    //            }
    //        }

    //        return Ok();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new { Errors = ex.ToString() });
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> SaveUserRole(RoleViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var result = new UserRole();

            foreach (var item in model.UserRole.UsersId)
            {
                var existingUserRole = await _userRoleRepo.GetUserRoleAsync(item);

                if (existingUserRole != null)
                {
                    existingUserRole.RoleId = model.UserRole.RoleId;
                    var mapped = _mapper.Map<UserRoleModel>(existingUserRole);
                    result = await _userRoleRepo.SaveAsync(mapped);
                }
                else
                {
                    // Create new user role if it doesn't exist
                    result = await _userRoleRepo.SaveAsync(new UserRoleModel()
                    {
                        RoleId = model.UserRole.RoleId,
                        UserId = item
                    });
                }
            }

            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveFromRole(string ids)
    {
        try
        {
            int[] _ids = Array.ConvertAll(ids.Split(','), int.Parse);
            await _userRoleRepo.BatchDeleteAsync(_ids);

            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveRole(RoleViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

            var result = await _roleRepo.SaveAsync(model.Role, model.RoleAccess);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRoles(string ids)
    {
        try
        {
            int[] _ids = Array.ConvertAll(ids.Split(','), int.Parse);
            await _roleRepo.BatchDeleteAsync(_ids);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion API Operation
}