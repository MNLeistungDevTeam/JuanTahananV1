using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.PropertyManagementRepo;

public interface IPropertyLocationRepository
{
    Task BatchDeleteAsync(int[] ids);
    Task<PropertyLocation> CreateAsync(PropertyLocation model, int userId);
    Task<List<PropertyLocation>> GetAll();
    Task<PropertyLocation?> GetById(int id);
    Task<IEnumerable<PropertyLocationModel?>> GetPropertyLocationByProjectAsync(int? projectId,int? developerId);
    Task<PropertyLocation> SaveAsync(PropertyLocationModel model, int userId);
    Task<PropertyLocation> UpdateAsync(PropertyLocation model, int userId);
}
