using DMS.Domain.Dto.PropertyTypeDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.PropertyTypeRepo
{
    public interface IPropertyTypeRepository
    {
        Task<IEnumerable<PropertyTypeModel>> GetAllAsync();
        Task<PropertyTypeModel?> GetAsync(int id);
        Task<PropertyType> SaveAsync(PropertyTypeModel model);
    }
}
