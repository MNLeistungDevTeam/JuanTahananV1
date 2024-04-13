using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.OtherDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

public class UserController : Controller
{
    #region Fields

    private readonly IUserRepository _userRepo;
    private readonly IAuthenticationService _authenticationService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileUploadService _fileUploadService;
    private readonly IUserRoleRepository _userRoleRepo;
    private readonly IRoleAccessRepository _roleAccessRepo;

    public UserController(
        IUserRepository userRepo,
        IAuthenticationService authenticationService,
        IWebHostEnvironment webHostEnvironment,
        IFileUploadService fileUploadService,
        IUserRoleRepository userRoleRepo,
        IRoleAccessRepository roleAccessRepo)
    {
        _userRepo = userRepo;
        _authenticationService = authenticationService;
        _webHostEnvironment = webHostEnvironment;
        _fileUploadService = fileUploadService;
        _userRoleRepo = userRoleRepo;
        _roleAccessRepo = roleAccessRepo;
    }

    #endregion Fields

    #region Views

    public async Task<IActionResult> Index()
    {
        try
        {
            var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_USERMNGMT);

            if (roleAccess is null) { return View("AccessDenied"); }
            if (!roleAccess.CanRead) { return View("AccessDenied"); }

            int userid = int.Parse(User.Identity.Name);

            var genders = new List<DropdownModel>
                {
                    new DropdownModel {Id = 1, Description = "Male"},
                    new DropdownModel {Id = 2, Description = "Female"}
                };

            var user = new UserModel
            {
                Genders = genders.ToList(),
            };

            var userApprover = new List<UserApproverModel>
        {
            new UserApproverModel{ ApproverId = 0 },
            new UserApproverModel{ ApproverId = 0 }
        };

            var userModel = new UserViewModel
            {
                User = user,
                UserApprover = userApprover
            };

            return View(userModel);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> Profile()
    {
        int userId = int.Parse(User.Identity.Name);

        var userdata = await _userRepo.GetUserAsync(userId)
            ?? throw new Exception("User not found!");

        var genders = new List<DropdownModel>
    {
        new() {Id = 1, Description = "Male"},
        new() {Id = 2, Description = "Female"}
    };

        userdata.Genders = genders;

        var userModel = new UserViewModel
        {
            User = userdata,
        };

        return View(userModel);
    }

    #endregion Views

    #region API Getters

    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var data = await _userRepo.GetUsersAsync();

            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var data = await _userRepo.GetUserAsync(id);

            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion API Getters

    #region API Operation

    [HttpPost]
    public async Task<IActionResult> UnlockUser(int userId)
    {
        try
        {
            await _authenticationService.UnlockUser(userId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveUser(UserViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
                return Conflict(errors);
            }

            var rootFolder = _webHostEnvironment.WebRootPath;
            string profileSaveLocation = Path.Combine("Files", "Images", "UserPics", model.User.UserName);
            string signatureSaveLocation = Path.Combine("Files", "Images", "UserSignatures", model.User.UserName);

            string? profilePicture = await _fileUploadService.SaveProfilePictureAsync(model.User?.ProfilePictureFile, model.User.UserName, profileSaveLocation, rootFolder);
            string? signature = await _fileUploadService.SaveFileAsync(model.User?.SignatureFile, signatureSaveLocation, rootFolder);

            model.User.ProfilePicture = profilePicture;
            model.User.Signature = signature;

            int userId = int.Parse(User.Identity.Name);

            var returnUserData = await _userRepo.SaveUserAsync(model.User, model.UserApprover, userId);

            UserRoleModel updateUserRole = new()
            {
                UserId = returnUserData.Id,
                RoleId = model.User.UserRoleId.Value
            };

            await _userRoleRepo.SaveAsync(updateUserRole);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
                return Conflict(errors);
            }

            await _authenticationService.ChangePassword(model);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion API Operation
}