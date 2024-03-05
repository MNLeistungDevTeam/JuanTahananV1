using DMS.Domain.Dto.UserDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DMS.Application.Services;

namespace DMS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BridgeController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public BridgeController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserModel user)
        {
            try
            {
                // Verification
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Status = 400,
                        Title = "Bad Request",
                        Detail = "Invalid user data."
                    });
                }
                user.Position = "Beneficiary";
                var userData = await _authService.RegisterUser(user);
                return Ok(new
                {
                    Status = 200,
                    Title = $"User Registration",
                    Detail = "User Created Successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                });
            }
        }


    }
}
