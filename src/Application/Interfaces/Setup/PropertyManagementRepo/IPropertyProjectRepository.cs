using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.PropertyManagementRepo;

public interface IPropertyProjectRepository
{
    Task BatchDeleteAsync(int[] ids);
    Task<PropertyProject> CreateAsync(PropertyProject model, int userId);
    Task<List<PropertyProject>> GetAll();
    Task<IEnumerable<PropertyProjectModel?>> GetAllAsync();
    Task<IEnumerable<PropertyProjectModel?>> GetByCompanyAsync(int companyId);
    Task<PropertyProject?> GetById(int id);
    Task<IEnumerable<PropertyProjectModel?>> GetPropertyLocationByProjectAsync(int id);
    Task<PropertyProject> SaveAsync(PropertyProjectModel model, int userId);
    Task SaveProjectLocations(PropertyProjectModel project, List<PropertyProjectLocationModel> userProjectList, int userId);
    Task<PropertyProject> UpdateAsync(PropertyProject model, int userId);
}
