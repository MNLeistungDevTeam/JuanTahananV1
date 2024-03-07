using AutoMapper;
using DMS.Application.Interfaces.Setup.AddressRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.EntityDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.AddressRepo
{

    public class AddressRepository : IAddressRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<Address> _contextHelper;
        private readonly ISQLDatabaseService _db;
        private readonly IMapper _mapper;

        public AddressRepository(
            DMSDBContext context,
            ISQLDatabaseService db,
            IMapper mapper)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Address>(_context);
            _db = db;
            _mapper = mapper;
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            var data = await _contextHelper.GetByIdAsync(id);

            return data;
        }

        public async Task<string?> GetCompanyDefaultAddress(int Id)
        {
            var address = string.Empty;
            var data = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(add => add.ReferenceType == (int)AddressReferenceType.Index.Company && add.ReferenceId == Id);
            if (data == null) return string.Empty;

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == data.CountryId);
            if (data != null && country != null)
            {
                address = string.Join(", ",
                data.StreetAddress1,
                data.CityMunicipality,
                country.CountryName,
                data.PostalCode);
            }
            return address;
        }

        public async Task<IEnumerable<AddressModel>> GetDefaultAddress(string code)
        {
            var data = await _db.LoadDataAsync<AddressModel, dynamic>("spAddress_GetDefaultByReferenceId", new { code });

            return data;
        }

        public async Task<List<Address>> GetByRefTypeId(int referenceType, int referenceId)
        {
            var data = _context.Addresses.Where(add => add.ReferenceType == referenceType && add.ReferenceId == referenceId).ToList();

            return data;
        }

        public async Task<List<Address>> GetAllAsync()
        {
            var data = await _contextHelper.GetAllAsync();

            return data;
        }

        public async Task<IEnumerable<AddressModel>> GetByReferenceId(int referenceId, int referenceType)
        {
            var data = await _db.LoadDataAsync<AddressModel, dynamic>("spAddress_GetByReferenceId", new { referenceId, referenceType });

            return data;
        }

        public async Task<Address> SaveAsync(AddressModel addressModel, int userId)
        {
            var _addressModel = _mapper.Map<Address>(addressModel);

            if (_addressModel.Id == 0)
            {
                _addressModel = await CreateAsync(_addressModel, userId);
            }
            else
            {
                _addressModel = await UpdateAsync(_addressModel, userId);
            }

            return _addressModel;
        }

        public async Task<Address> CreateAsync(Address address, int createdById)
        {
            address.CreatedById = createdById;
            address.DateCreated = DateTime.Now;
            var result = await _contextHelper.CreateAsync(address, "ModifiedById", "DateModified");

            return result;
        }

        public async Task<Address> UpdateAsync(Address address, int modifiedById)
        {
            address.ModifiedById = modifiedById;
            address.DateModified = DateTime.Now;
            var result = await _contextHelper.UpdateAsync(address, "CreatedById", "DateCreated");

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.Addresses.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.Addresses.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(List<Address> addresses)
        {
            await _contextHelper.BatchDeleteAsync(addresses);
        }
    }
}
