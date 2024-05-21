using DMS.Application.Services;
using DMS.Domain.Dto.BasicBeneficiaryDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
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

        #region Api For JTAHANAN-PH Portal

        [HttpGet("GetDevelopers")]
        public async Task<IActionResult> GetDevelopers()
        {
            try
            {
                var developers = await _zetaHousingLoanIntegrationService.GetDevelopers();

                return Ok(developers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProjectsByCompany")]
 

        public async Task<IActionResult> GetProjectsByCompany(int companyId, int? locationId)
        {
            try
            {
                var projects = await _zetaHousingLoanIntegrationService.GetProjectsByCompany(companyId, locationId);

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetLocationsByProject")]
        public async Task<IActionResult> GetLocationsByProject(int? projectId, int? developerId)
        {
            try
            {
                var projects = await _zetaHousingLoanIntegrationService.GetLocationsByProject(projectId, developerId);

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUnitsByProject")]
        public async Task<IActionResult> GetUnitsByProject(int? projectId,int? developerId)
        {
            try
            {
                var projects = await _zetaHousingLoanIntegrationService.GetUnitsByProject(projectId,developerId);

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion Api For JTAHANAN-PH Portal





    }
}