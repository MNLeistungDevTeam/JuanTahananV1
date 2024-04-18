using DMS.Application.Services;
using DMS.Domain.Dto.BasicBeneficiaryDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Api
{
    [Route("api/HousingLoanApplication")]
    [ApiController]
    public class HousingLoanApplicantApiController : ControllerBase
    {
        private readonly IHousingLoanIntegrationService _zetaHousingLoanIntegrationService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HousingLoanApplicantApiController(IHousingLoanIntegrationService zetaHousingLoanIntegrationService, IWebHostEnvironment webHostEnvironment)
        {
            _zetaHousingLoanIntegrationService = zetaHousingLoanIntegrationService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("SaveBenificiary")]
        public async Task<IActionResult> SaveBeneficiaryAsync([FromBody] BasicBeneficiaryInformationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //default the first company use
                //int companyId = int.TryParse(User.FindFirstValue("Company"), out int result) ? result : 1;

                int companyId = 1;

                model.CompanyId = companyId;
                await _zetaHousingLoanIntegrationService.SaveBeneficiaryAsync(model, _webHostEnvironment.WebRootPath);

                return Ok("Save Beneficiary Successfuly!"); // or return any other appropriate response
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}