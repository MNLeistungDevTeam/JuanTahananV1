using AutoMapper;
using DMS.Application.Interfaces.Setup.AddressTypeRepo;
using DMS.Application.Services;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.AddressTypeRepo
{
    public class AddressTypeRepository : IAddressTypeRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<AddressType> _contextHelper;
        private readonly ISQLDatabaseService _db;
        private readonly IMapper _mapper;

        public AddressTypeRepository(
            DMSDBContext context,
            ISQLDatabaseService db,
            IMapper mapper)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<AddressType>(_context);
            _db = db;
            _mapper = mapper;
        }

        public async Task<AddressType?> GetByIdAsync(int id)
        {
            var data = await _contextHelper.GetByIdAsync(id);

            return data;
        }

        public async Task<List<AddressType>> GetAllAsync()
        {
            var data = await _contextHelper.GetAllAsync();

            return data;
        }

        public async Task<AddressType> SaveAsync(AddressType addressModel, int userId)
        {
            if (addressModel.Id == 0)
            {
                addressModel = await CreateAsync(addressModel, userId);
            }
            else
            {
                addressModel = await UpdateAsync(addressModel, userId);
            }

            return addressModel;
        }

        public async Task<AddressType> CreateAsync(AddressType address, int createdById)
        {
            address.CreatedBy = createdById;
            address.DateCreated = DateTime.UtcNow;
            var result = await _contextHelper.CreateAsync(address, "ModifiedById", "DateModified");

            return result;
        }

        public async Task<AddressType> UpdateAsync(AddressType address, int modifiedById)
        {
            address.ModifiedBy = modifiedById;
            address.DateModified = DateTime.UtcNow;
            var result = await _contextHelper.UpdateAsync(address, "CreatedById", "DateCreated");

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.AddressTypes.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.AddressTypes.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }
    }
}
