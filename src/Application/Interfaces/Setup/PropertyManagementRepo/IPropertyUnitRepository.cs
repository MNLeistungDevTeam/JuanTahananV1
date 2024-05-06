using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.PropertyManagementRepo;

public interface IPropertyUnitRepository
{
    Task BatchDeleteAsync(int[] ids);
    Task<PropertyUnit> CreateAsync(PropertyUnit model, int userId);
    Task<List<PropertyUnit>> GetAll();
    Task<PropertyUnit?> GetById(int id);
    Task<PropertyUnit> SaveAsync(PropertyUnitModel model, int userId);
    Task<PropertyUnit> UpdateAsync(PropertyUnit model, int userId);
}
