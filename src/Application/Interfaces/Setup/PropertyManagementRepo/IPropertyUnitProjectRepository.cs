using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.PropertyManagementRepo;

public interface IPropertyUnitProjectRepository
{
    Task BatchDeleteAsync(int[] ids);
    Task<PropertyUnitProject> CreateAsync(PropertyUnitProject model, int userId);
    Task<List<PropertyUnitProject>> GetAll();
    Task<PropertyUnitProject?> GetById(int id);
    Task<PropertyUnitProject> SaveAsync(PropertyUnitProjectModel model, int userId);
    Task<PropertyUnitProject> UpdateAsync(PropertyUnitProject model, int userId);
}
