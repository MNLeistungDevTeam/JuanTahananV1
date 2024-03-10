using DMS.Application.Interfaces.Setup.CountryRepo;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup
{
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepo;

        public CountryController(ICountryRepository countryRepo)
        {
            _countryRepo = countryRepo;
        }

        public async Task<IActionResult> GetCountries()
        {
            var data = await _countryRepo.GetAllAsync();
            return Ok(data);
        }
    }
}
