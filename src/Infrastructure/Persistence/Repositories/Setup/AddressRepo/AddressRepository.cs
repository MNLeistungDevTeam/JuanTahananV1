using DMS.Application.Interfaces.Setup.AddressRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.EntityDto;
using DMS.Domain.Entities;
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

        public AddressRepository(DMSDBContext context, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Address>(context);
            _db = db;
        }

        #region Get Methods

        public async Task<AddressModel> GetByRefIdAsync(int referenceId, int referenceType) =>
            await _db.LoadSingleAsync<AddressModel, dynamic>("spAddress_GetByReferenceId", new { referenceId, referenceType });


        public async Task<Address?> GetByRefId2Async(int referenceId, int referenceType) =>
            await _context.Addresses.FirstOrDefaultAsync(a => a.ReferenceId == referenceId && a.ReferenceType == referenceType);

        #endregion Get Methods

        #region Operation Methods

        public async Task<Address> SaveAsync(Address address, int userId)
        {
            if (address.Id == 0)
                address = await CreateAsync(address, userId);
            else
                address = await UpdateAsync(address, userId);

            return address;
        }

        public async Task<Address> CreateAsync(Address address, int userId)
        {
            address.CreatedById = userId;
            address.DateCreated = DateTime.Now;

            var result = await _contextHelper.CreateAsync(address, "ModifiedById", "DateModified");

            return result;
        }

        public async Task<Address> UpdateAsync(Address address, int userId)
        {
            address.ModifiedById = userId;
            address.DateModified = DateTime.Now;

            var result = await _contextHelper.UpdateAsync(address, "CreatedById", "DateCreated");

            return result;
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.Addresses.Where(a => ids.Contains(a.Id));

            await _contextHelper.BatchDeleteAsync(entities);
        }

        #endregion Operation Methods
    }
}
