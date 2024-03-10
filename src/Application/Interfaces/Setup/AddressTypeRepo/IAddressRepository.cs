using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.AddressTypeRepo
{
    public interface IAddressTypeRepository
    {
        Task BatchDeleteAsync(int[] ids);

        Task<AddressType> CreateAsync(AddressType address, int createdById);

        Task DeleteAsync(int id);

        Task<List<AddressType>> GetAllAsync();

        Task<AddressType?> GetByIdAsync(int id);

        Task<AddressType> SaveAsync(AddressType addressModel, int userId);

        Task<AddressType> UpdateAsync(AddressType address, int modifiedById);
    }
}
