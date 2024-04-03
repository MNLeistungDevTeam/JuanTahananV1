using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.BeneficiaryInformationDto
{
    public class BeneficiaryInformationModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [Display(Name = "Pagibig Number", Prompt = "Pagibig Number")]
        public string PagibigNumber { get; set; }

        public int? CompanyId { get; set; }
        [Display(Name = "Last Name", Prompt = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name", Prompt = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name", Prompt = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Birth Date", Prompt = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        public int? Age { get; set; }

        public string MobileNumber { get; set; }

        public string Sex { get; set; }

        public bool IsPermanentAddressAbroad { get; set; }

        public bool IsPresentAddressAbroad { get; set; }

        public string PresentUnitName { get; set; }

        public string PresentBuildingName { get; set; }

        public string PresentLotName { get; set; }

        public string PresentStreetName { get; set; }

        public string PresentSubdivisionName { get; set; }

        public string PresentBaranggayName { get; set; }

        public string PresentMunicipalityName { get; set; }

        public string PresentProvinceName { get; set; }

        public string PresentZipCode { get; set; }

        public string PermanentUnitName { get; set; }

        public string PermanentBuildingName { get; set; }

        public string PermanentLotName { get; set; }

        public string PermanentStreetName { get; set; }

        public string PermanentSubdivisionName { get; set; }

        public string PermanentBaranggayName { get; set; }

        public string PermanentMunicipalityName { get; set; }

        public string PermanentProvinceName { get; set; }

        public string PermanentZipCode { get; set; }

        public string PropertyDeveloperName { get; set; }

        public string PropertyLocation { get; set; }

        public string PropertyUnitLevelName { get; set; }

        public int? CreatedById { get; set; }

        public DateTime DateCreated { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateModified { get; set; }
        public string? Email { get; set; }
    }
}
