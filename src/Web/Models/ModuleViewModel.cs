using DMS.Domain.Dto.ModuleDto;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Dto.ModuleTypeDto;
using System.Collections.Generic;



namespace DMS.Web.Models
{
    public class ModuleViewModel
    {
        public ModuleModel Module { get; set; } = new();
        public ModuleTypeModel ModuleType { get; set; } = new();
        public List <ModuleStageModel> ModuleStages { get; set; } = new();
    }
}