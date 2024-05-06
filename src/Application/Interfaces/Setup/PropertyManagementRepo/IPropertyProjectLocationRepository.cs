using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.PropertyManagementRepo;

public interface IPropertyProjectLocationRepository
{
    Task BatchDeleteAsync(int[] ids);
    Task<PropertyProjectLocation> CreateAsync(PropertyProjectLocation model, int userId);
    Task<List<PropertyProjectLocation>> GetAll();
    Task<PropertyProjectLocation?> GetById(int id);
    Task<List<PropertyProjectLocation>> GetbyProjectId(int id);
    Task<PropertyProjectLocation> SaveAsync(PropertyProjectLocationModel model, int userId);
    Task<PropertyProjectLocation> UpdateAsync(PropertyProjectLocation model, int userId);
}
