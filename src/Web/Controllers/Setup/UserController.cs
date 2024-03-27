﻿using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.OtherDto;
using DMS.Domain.Dto.UserDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileUploadService _fileUploadService;
        private readonly IUserRoleRepository _userRoleRepo;

        public UserController(
            IUserRepository userRepo,
            IAuthenticationService authenticationService,
            IWebHostEnvironment webHostEnvironment,
            IFileUploadService fileUploadService,
            IUserRoleRepository userRoleRepo)
        {
            _userRepo = userRepo;
            _authenticationService = authenticationService;
            _webHostEnvironment = webHostEnvironment;
            _fileUploadService = fileUploadService;
            _userRoleRepo = userRoleRepo;
        }

        public IActionResult Index()
        {
            try
            {
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

                if (model.User.Id == 0)
                {
                    UserRoleModel userRole = new()
                    {
                        UserId = returnUserData.Id,
                        RoleId = model.User.UserRoleId.Value
                    };

                    await _userRoleRepo.SaveAsync(userRole);
                }

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
    }
}