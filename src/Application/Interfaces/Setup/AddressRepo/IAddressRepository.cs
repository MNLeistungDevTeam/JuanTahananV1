using DMS.Domain.Dto.EntityDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.AddressRepo
{

    public interface IAddressRepository
    {
        Task<IEnumerable<AddressModel>> GetDefaultAddress(string code);

        Task<string?> GetCompanyDefaultAddress(int Id);

        Task BatchDeleteAsync(List<Address> addresses);

        Task BatchDeleteAsync(int[] ids);

        Task<Address> CreateAsync(Address address, int createdById);

        Task DeleteAsync(int id);

        Task<List<Address>> GetAllAsync();

        Task<Address?> GetByIdAsync(int id);

        Task<IEnumerable<AddressModel>> GetByReferenceId(int referenceId, int referenceType);

        Task<List<Address>> GetByRefTypeId(int referenceType, int referenceId);

        Task<Address> SaveAsync(AddressModel addressModel, int userId);

        Task<Address> UpdateAsync(Address address, int modifiedById);
    }
}
