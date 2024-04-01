using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.Authentication;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Hubs;
using DMS.Web.Controllers.Services;
using DMS.Web.Models;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Authentication;

[Authorize]
public class AccountController : Controller
{
    private readonly Application.Services.IAuthenticationService _authService;
    private readonly IHubContext<AuthenticationHub> _hubContext;
    private readonly IJwtService _jwtService;
    private readonly IUserTokenRepository _userTokenRepo;
    private readonly ICompanyRepository _companyRepo;
    private IUserRepository _userRepo;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IEmailService _emailService;

    public AccountController(
        Application.Services.IAuthenticationService authService,
        IHubContext<AuthenticationHub> hubContext,
        IJwtService jwtService,
        IUserTokenRepository userTokenRepo,
        ICompanyRepository companyRepo,
        IUserRepository userRepo,
        IBackgroundJobClient backgroundJobClient,
        IEmailService emailService)
    {
        _authService = authService;
        _hubContext = hubContext;
        _jwtService = jwtService;
        _userTokenRepo = userTokenRepo;
        _companyRepo = companyRepo;
        _userRepo = userRepo;
        _backgroundJobClient = backgroundJobClient;
        _emailService = emailService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Login(string returnUrl)
    {
        try
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var companies = await _companyRepo.GetCompanies();

            var companylist = companies.ToList();

            var viewModel = new LoginViewModel
            {
                Company = companylist,
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("error", new ErrorViewModel
            {
                Message = ex.Message,
                Exception = ex
            });
        }
    }

    [AllowAnonymous]
    public IActionResult AuthRecover()
    {
        return View("AuthRecover");
    }

    // [AllowAnonymous]
    // public async Task<IActionResult> Register(UserModel user)
    // {
    //     try
    //     {
    //         // Verification
    //         if (!ModelState.IsValid)
    //         {
    //             var validationError = ModelState
    //                 .Where(x => x.Value.Errors.Any())
    //                 .Select(x => new { x.Key, x.Value.Errors });
    //
    //             return Conflict(validationError);
    //         }
    //
    //         var userData = await _authService.RegisterUser(user);
    //
    //         return View(userData);
    //     }
    //     catch (Exception ex) { return View("error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    // }

    [AllowAnonymous]
    public async Task<IActionResult> LogOffAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await _hubContext.Clients.All.SendAsync("CheckIfAuthenticated");
        return RedirectToAction("Login", "Account");
    }

    [ModuleServices(ModuleCodes.Profile, typeof(IModuleRepository))]
    public IActionResult Profile()
    {
        return View();
    }

    #region API

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginViewModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verification.
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request!");
            }

            AuthRequest authRequest = new()
            {
                UserName = model.UserName,
                Password = model.Password
            };
            var result = await _authService.Authenticate(authRequest);

            model.Id = result.Id;

            await SignInAsync(model);

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpGet("{action}/{token}")]
    public async Task<IActionResult> LoginTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new AuthResponse { IsSuccess = false, Reason = "Token must be provided." });
        }

        var result = await _jwtService.Authenticate(token);
        LoginViewModel loginViewModel = new()
        {
            UserName = result.UserName,
        };

        await SignInAsync(loginViewModel);
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AuthToken([FromBody] AuthRequest authRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponse
            {
                IsSuccess = false,
                Reason = "UserName and Password must be provided."
            });
        }

        var authResponse = await _jwtService.GetTokenAsync(authRequest);
        if (authResponse == null)
        {
            return Unauthorized();
        }

        return Ok(authResponse);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponse
            {
                IsSuccess = false,
                Reason = "Tokens must be provided"
            });
        }

        //string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        var token = GetJwtToken(request.ExpiredToken);
        var userRefreshToken = await _userTokenRepo.GetTokenAsync(request.ExpiredToken, request.RefreshToken);

        if (userRefreshToken == null)
        {
            return NotFound(new AuthResponse
            {
                IsSuccess = false,
                Reason = "Token not found!"
            });
        }

        AuthResponse response = ValidateDetails(token, userRefreshToken);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        userRefreshToken.IsInvalidated = true;
        await _userTokenRepo.UpdateAsync(userRefreshToken);

        // var userName = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value;
        var authResponse = await _jwtService.GetRefreshTokenAsync(userRefreshToken.UserId);

        return Ok(authResponse);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RecoverCredentials(LoginViewModel model)
    {
        try
        {
            //// Verification.
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Invalid Request!");
            //}
            var users = await _userRepo.GetUsersAsync();
            var userWithEmail = users.FirstOrDefault(user => user.Email == model.RecoveryEmail);

            var userdata = await _userRepo.GetUserAsync(userWithEmail.Id);

            if (userWithEmail != null)
            {
               // userdata.Password = _authService.GenerateTemporaryPasswordAsync(userdata.FirstName); //sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0

                userdata.Action = "reset";
              // await _authService.ResetCredential(userdata);

                //// make the usage of hangfire
                _backgroundJobClient.Enqueue(() => _emailService.SendUserConfirmationMessage(userdata));
            }

            return Ok();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion API

    #region Private Helpers

    private static AuthResponse ValidateDetails(JwtSecurityToken token, UserToken userRefreshToken)
    {
        if (userRefreshToken == null)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Reason = "Invalid Token Details."
            };
        }

        if (token.ValidTo > DateTime.UtcNow)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Reason = "Token not expired."
            };
        }

        if (userRefreshToken.ExpirationDate > DateTime.UtcNow)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Reason = "Refresh Token Expired"
            };
        }
        return new AuthResponse
        {
            IsSuccess = true
        };
    }

    private static JwtSecurityToken GetJwtToken(string expiredToken)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        return tokenHandler.ReadJwtToken(expiredToken);
    }

    private async Task SignInAsync(LoginViewModel model)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Id.ToString()),
            new Claim(ClaimTypes.Role, model.Id.ToString()),
             new Claim("Company", model.CompanyId != null ? model.CompanyId.ToString() : "0")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            IsPersistent = model.RememberMe
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction("Index", "Home");
    }

    #endregion Private Helpers
}