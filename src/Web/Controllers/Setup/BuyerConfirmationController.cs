using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Domain.Dto.BeneficiaryInformationDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup
{
    public class BuyerConfirmationController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IBeneficiaryInformationRepository _beneficiaryInformationRepo;
        private readonly IBuyerConfirmationRepository _buyerConfirmationRepo;

        public BuyerConfirmationController(IUserRepository userRepo, IBeneficiaryInformationRepository beneficiaryInformationRepo, IBuyerConfirmationRepository buyerConfirmationRepo)
        {
            _userRepo = userRepo;
            _beneficiaryInformationRepo = beneficiaryInformationRepo;
            _buyerConfirmationRepo = buyerConfirmationRepo;
        }

        [Route("[controller]/LatestBCF")]
        public async Task<IActionResult> LatestBCF()
        {
            try
            {
                ApplicantViewModel vwModel = new();

                int userId = int.Parse(User.Identity.Name);

                var userData = await _userRepo.GetUserAsync(userId);

                var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(userData.PagibigNumber);

                vwModel.BuyerConfirmationModel.UserId = userData.Id;
                vwModel.BuyerConfirmationModel.PagibigNumber = beneficiaryData.PagibigNumber;

                vwModel.BuyerConfirmationModel.FirstName = beneficiaryData.FirstName ?? string.Empty;
                vwModel.BuyerConfirmationModel.MiddleName = beneficiaryData.MiddleName ?? string.Empty;
                vwModel.BuyerConfirmationModel.LastName = beneficiaryData.LastName ?? string.Empty;

                vwModel.BuyerConfirmationModel.Email = beneficiaryData.Email;

                vwModel.BuyerConfirmationModel.Suffix = userData.Suffix;

                return View(vwModel);
            }
            catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
        }

        [HttpPost]
        public async Task<IActionResult> SaveBCF(ApplicantViewModel vwModel)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
                //}

                int userId = int.Parse(User.Identity.Name);

                //Unmasked
                vwModel.BuyerConfirmationModel.PagibigNumber = vwModel.BuyerConfirmationModel.PagibigNumber.Replace("-", "") ?? string.Empty;

                var bcfData = await _buyerConfirmationRepo.SaveAsync(vwModel.BuyerConfirmationModel, userId);

                return Ok(bcfData.Code);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}