using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Dto.EntityDto;
using System.Collections.Generic;

namespace DMS.Web.Models
{
    public class CompanyViewModel
    {
        public CompanyModel Company { get; set; }
        public CompanySettingModel Setting { get; set; }
        public List<CompanyLogoModel> CompanyLogo { get; set; }
        public List<AddressModel> Address { get; set; }

        public CompanyViewModel()
        {
            Company = new();
            Setting = new();
            CompanyLogo = new();
            Address = new();
        }
    }
}
