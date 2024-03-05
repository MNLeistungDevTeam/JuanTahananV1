using AutoMapper;
using DevExpress.DirectX.Common.DirectWrite;
using DevExpress.Drawing.Internal.Fonts.Interop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Interfaces.Setup.ModuleRepository;
using Template.Application.Interfaces.Setup.RoleRepository;
using Template.Application.Interfaces.Setup.UserRepository;
using Template.Domain.Dto.ModuleDto;
using Template.Domain.Dto.RoleDto;
using Template.Domain.Dto.UserDto;
using Template.Domain.Entities;
using Template.Domain.Enums;
using Template.Web.Controllers.Services;
using Template.Web.Models;

namespace Template.Web.Controllers.Setup
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IRoleAccessRepository _roleAccessRepo;
        private readonly IModuleRepository _moduleRepo;
        private readonly IMapper _mapper;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IUserRepository userRepo;
        public RoleController(IRoleRepository roleRepo, IRoleAccessRepository roleAccessRepo, IModuleRepository moduleRepo, IMapper mapper, IUserRoleRepository userRoleRepo, IUserRepository userRepo)
        {
            _roleRepo = roleRepo;
            _roleAccessRepo = roleAccessRepo;
            _moduleRepo = moduleRepo;
            _mapper = mapper;
            _userRoleRepo = userRoleRepo;
            this.userRepo = userRepo;
        }
        [ModuleServices(ModuleCodes.Role, typeof(IModuleRepository))]
        public IActionResult Index()
        {
            return View();
        }
        [ModuleServices(ModuleCodes.UserRole, typeof(IModuleRepository))]
        public async Task<IActionResult> UserRole(int id)
        {
            return View(new RoleViewModel()
            {
                Role = _mapper.Map<RoleModel>(await _roleRepo.GetByIdAsync(id))
            });
        }
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
                    canModify : roleaccess.CanModify,
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
        [HttpPost]
        [ModelStateValidations(typeof(RoleViewModel))]
        public async Task<IActionResult> SaveUserRole(RoleViewModel model)
        {
            try
            {
                foreach (var item in model.UserRole.UsersId)
                {
                    var existingUserRole = await _userRoleRepo.GetUserRoleAsync(item);

                    if (existingUserRole != null)
                    {
                        // Update existing user role if it already exists
                        existingUserRole.RoleId = model.UserRole.RoleId;
                        await _userRoleRepo.SaveAsync(_mapper.Map<UserRoleModel>(existingUserRole));
                    }
                    else
                    {
                        // Create new user role if it doesn't exist
                        await _userRoleRepo.SaveAsync(new UserRoleModel()
                        {
                            RoleId = model.UserRole.RoleId,
                            UserId = item
                        });
                    }
                }

                return Ok("added");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = ex.ToString() });
            }
        }
        [HttpPost]
        [ModelStateValidations(typeof(RoleViewModel))]
        public async Task<IActionResult> SaveRole(RoleViewModel model)
        {
            try
            {
                if (model.Role.Id == 0)
                {
                    // Add new role logic
                    var role = await _roleRepo.SaveAsync(model.Role);
                    if (role != null)
                    {
                        foreach (var item in model.RoleAccess)
                        {
                            item.RoleId = role.Id;
                            await _roleAccessRepo.SaveAsync(item);
                        }
                        return Ok("added");
                    }
                    return BadRequest(new { Errors = "Adding role failed." });
                }
                else
                {
                    // Update existing role logic
                    var role = await _roleRepo.GetByIdAsync(model.Role.Id);
                    if (role == null)
                    {
                        return NotFound(new { Message = "Role not found." });
                    }
                    model.Role.IsLocked = role.IsLocked; // Ensure IsLocked is properly handled
                   await _roleRepo.SaveAsync(model.Role);

                    // Update RoleAccess items
                    foreach (var item in model.RoleAccess)
                    {
                        var existingRoleAccess = await _roleAccessRepo.GetRoleAccessAsync(role.Id, item.ModuleId);
                        if (existingRoleAccess == null)
                        {
                            // Create new RoleAccess item if not found
                            existingRoleAccess = new RoleAccess
                            {
                                RoleId = role.Id,
                                ModuleId = item.ModuleId
                            };
                        }
                        // Update properties
                        existingRoleAccess.CanCreate = item.CanCreate;
                        existingRoleAccess.CanDelete = item.CanDelete;
                        existingRoleAccess.CanRead = item.CanRead;
                        existingRoleAccess.CanModify = item.CanModify;
                        existingRoleAccess.ModuleId = item.ModuleId;
                        await _roleAccessRepo.SaveAsync(_mapper.Map<RoleAccessModel>(existingRoleAccess));
                    }
                    return Ok("updated");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = ex.ToString()});
            }
        }

        public async Task DeleteRoles(int[] ids) =>
            await _roleRepo.BatchDeleteAsync(ids);
        public async Task<IActionResult> GetRoleById(int id) =>
            Ok(await _roleRepo.GetByIdAsync(id));
        public async Task<IActionResult> GetAllRoles() =>
            Ok(await _roleRepo.SpGetAllRoles());

        public async Task<IActionResult> GetUsers(int roleId)
        {
            var userRoles = (await _userRoleRepo.GetAllAsync()).Where(x => x.RoleId == roleId).Select(s => s.UserId).ToList();

            List<UserModel> usersWithoutRole = (await userRepo.GetUsersAsync()).ToList();

            if (userRoles.Any())
            {
                usersWithoutRole = usersWithoutRole.Where(x => !userRoles.Contains(x.Id)).ToList();
            }

            return Ok(usersWithoutRole.Select(x => new {
                text = x.Name,
                value = x.Id,
                position = x.Position
            }));
        }


        public async Task<IActionResult> GetUserRoles(int RoleId) =>
            Ok(((await _userRoleRepo.SpGetAllRoles())).Where(x => x.RoleId == RoleId));
        public async Task RemoveFromRole(int[] ids) =>
            await _userRoleRepo.BatchDeleteAsync(ids);
    }
}
