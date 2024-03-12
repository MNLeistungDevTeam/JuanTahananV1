using DMS.Application.Services;
using DMS.Domain.Dto.BasicBeneficiaryDto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Api
{
    [Route("api/HousingLoanApplication")]
    [ApiController]
    public class HousingLoanApplicantApiController : ControllerBase
    {
        private readonly IHousingLoanIntegrationService _zetaHousingLoanIntegrationService;

        public HousingLoanApplicantApiController(IHousingLoanIntegrationService zetaHousingLoanIntegrationService)
        {
            _zetaHousingLoanIntegrationService = zetaHousingLoanIntegrationService;
        }

        [HttpPost("SaveBenificiary")]
        public async Task<IActionResult> SaveBeneficiaryAsync([FromBody] BasicBeneficiaryInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _zetaHousingLoanIntegrationService.SaveBeneficiaryAsync(model);

            return Ok(""); // or return any other appropriate response
        }
    }
}