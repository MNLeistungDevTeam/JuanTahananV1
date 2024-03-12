using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DMS.Application.Interfaces.Setup.UserRepository;

namespace DMS.Web.Controllers.Setup
{
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;   
                
        }
        public IActionResult Index()
        {
            return View();
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
    }
}
