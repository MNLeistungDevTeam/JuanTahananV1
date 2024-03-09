using AutoMapper;
using DMS.Application.Interfaces.Setup.PropertyTypeRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.PropertyTypeDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.PropertyTypeRepo
{
    public class PropertyTypeRepository :IPropertyTypeRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<PropertyType> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public PropertyTypeRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<PropertyType>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<IEnumerable<PropertyTypeModel>> GetAllAsync() =>
            await _db.LoadDataAsync<PropertyTypeModel, dynamic>("spPropertyType_GetAll", new { });

        public async Task<PropertyTypeModel?> GetAsync(int id) =>
            await _db.LoadSingleAsync<PropertyTypeModel, dynamic>("spPropertyType_Get", new { id });

        public async Task<PropertyType> SaveAsync(PropertyTypeModel model)
        {
            var propertyType = _mapper.Map<PropertyType>(model);

            if (model.Id == 0)
                propertyType = await CreateAsync(propertyType);
            else
                propertyType = await UpdateAsync(propertyType);

            return propertyType;
        }

        public async Task<PropertyType> CreateAsync(PropertyType propertyType)
        {
            try
            {
                propertyType.CreatedById = _currentUserService.GetCurrentUserId();
                propertyType.DateCreated = DateTime.Now;
                var result = await _contextHelper.CreateAsync(propertyType, "");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PropertyType> UpdateAsync(PropertyType propertyType)
        {
            try
            {
                var result = await _contextHelper.UpdateAsync(propertyType, "CreatedById", "DateCreated");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}