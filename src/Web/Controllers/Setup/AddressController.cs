using DMS.Application.Interfaces.Setup.AddressRepo;
using DMS.Application.Interfaces.Setup.AddressTypeRepo;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup
{
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepo;
        private readonly IAddressTypeRepository _addressTypeRepo;

        public AddressController(
            IAddressRepository addressRepo,
            IAddressTypeRepository addressTypeRepo)
        {
            _addressRepo = addressRepo;
            _addressTypeRepo = addressTypeRepo;
        }

        public async Task<IActionResult> GetAddressTypes()
        {
            var data = await _addressTypeRepo.GetAllAsync();
            return Ok(data);
        }
        public async Task<IActionResult> GetAddressById(int Id)
        {
            var data = await _addressRepo.GetByIdAsync(Id);
            return Ok(data);
        }
        public async Task<IActionResult> GetAddressByRefId(int referenceType, int referenceId)
        {
            var data = await _addressRepo.GetByRefTypeId(referenceType, referenceId);
            return Ok(data);
        }
    }
}
