using DMS.Application.Services;
using DMS.Domain.Dto.ZetaHousingLoanModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Api
{
    [Route("api/HousingLoanApplication")]
    [ApiController]
    public class HousingLoanApplicantApiController : ControllerBase
    {
        private readonly IZetaHousingLoanIntegrationService _zetaHousingLoanIntegrationService;

        public HousingLoanApplicantApiController(IZetaHousingLoanIntegrationService zetaHousingLoanIntegrationService)
        {
            _zetaHousingLoanIntegrationService = zetaHousingLoanIntegrationService;
        }

        [HttpPost("SaveBenificiary")]
        public async Task<IActionResult> SaveBeneficiaryAsync([FromBody] ZetaHousingLoanModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _zetaHousingLoanIntegrationService.SaveBeneficiaryAsync(model);

            return Ok(); // or return any other appropriate response
        }
    }
}