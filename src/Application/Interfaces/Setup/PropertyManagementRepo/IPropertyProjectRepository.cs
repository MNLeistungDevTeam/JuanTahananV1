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
    Task<PropertyProject?> GetById(int id);
    Task<PropertyProject> SaveAsync(PropertyProjectModel model, int userId);
    Task<PropertyProject> UpdateAsync(PropertyProject model, int userId);
}
