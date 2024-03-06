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
        Task<Address> CreateAsync(Address address, int userId);

        Task<Address> SaveAsync(Address address, int userId);

        Task<Address> UpdateAsync(Address address, int userId);

        Task BatchDeleteAsync(int[] ids);

        Task<AddressModel> GetByRefIdAsync(int referenceId, int referenceType);
        Task<Address?> GetByRefId2Async(int referenceId, int referenceType);
    }
}
