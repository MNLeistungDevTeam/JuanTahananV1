using DMS.Domain.Dto.PropertyManagementDto;

namespace DMS.Web.Models;

public class PropertyManagementViewModel
{
    public PropertyLocationModel? PropLocModel { get; set; }

    public PropertyProjectLocationModel? PropProjLocModel { get; set; }

    public PropertyProjectModel? PropProjModel { get; set; }

    public PropertyUnitModel? PropUnitModel { get; set; }

    public PropertyUnitProjectModel? PropUnitProjModel { get; set; }
}