using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApplicantsDto
{
    public class SpouseModel
    {
        public int Id { get; set; }

        [DisplayName("Applicants Personal Information Id")]
        public int? ApplicantsPersonalInformationId { get; set; }

        [Display(Name = "Is Spouse Address Abroad", Prompt = "Address Abroad")]
        public bool IsSpouseAddressAbroad { get; set; }

        [Display(Name = "Employment Unit Name", Prompt = "Unit Name")]
        public string? SpouseEmploymentUnitName { get; set; }

        [Display(Name = "Employment Building Name", Prompt = "Building Name")]
        public string? SpouseEmploymentBuildingName { get; set; }

        [Display(Name = "Employment Lot Name", Prompt = "Lot Name")]
        public string? SpouseEmploymentLotName { get; set; }

        [Display(Name = "Employment Street Name", Prompt = "Street Name")]
        public string? SpouseEmploymentStreetName { get; set; }

        [Display(Name = "Employment Subdivision Name", Prompt = "Subdivision Name")]
        public string? SpouseEmploymentSubdivisionName { get; set; }

        [Display(Name = "Employment Baranggay Name", Prompt = "Baranggay Name")]
        public string? SpouseEmploymentBaranggayName { get; set; }

        [Display(Name = "Employment Municipality Name", Prompt = "Municipality Name")]
        public string? SpouseEmploymentMunicipalityName { get; set; }

        [Display(Name = "Employment Province Name", Prompt = "Province Name")]
        public string? SpouseEmploymentProvinceName { get; set; }

        [Display(Name = "Employment Zip Code", Prompt = "Zip Code")]
        public string? SpouseEmploymentZipCode { get; set; }

        [Display(Name = "Prepared Mailing Address", Prompt = "Mailing Address")]
        public string? PreparedMailingAddress { get; set; }

        [Display(Name = "Preferred Time To Contact", Prompt = "Time To Contact")]
        public DateTime? PreferredTimeToContact { get; set; }

        [Display(Name = "Last Name", Prompt = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "First Name", Prompt = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name", Prompt = "Middle Name")]
        public string? MiddleName { get; set; }

        [Display(Name = "Suffix", Prompt = "Suffix")]
        public string? Suffix { get; set; }

        [Display(Name = "Pagibig Mid Number", Prompt = "XXXX-XXXX-XXXX")]
        public string? PagibigMidNumber { get; set; }

        [Display(Name = "Tin Number", Prompt = "XXX-XXX-XXX-XXXX")]
        public string? TinNumber { get; set; }

        [Display(Name = "Citizenship", Prompt = "Citizenship")]
        public string? Citizenship { get; set; }

        [Display(Name = "Birth Date", Prompt = "Birth Date")]
        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]

        public DateTime? BirthDate { get; set; }

        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; }

        [DisplayName("Created By Id")]
        public int? CreatedById { get; set; }

        [DisplayName("Date Modified")]
        public DateTime? DateModified { get; set; }

        [DisplayName("Modified By Id")]
        public int? ModifiedById { get; set; }

        [DisplayName("Date Deleted")]
        public DateTime? DateDeleted { get; set; }

        [Display(Name = "Business Number", Prompt = "Business Number")]
        public int? BusinessNumber { get; set; }

        [DisplayName("Deleted By Id")]
        public int? DeletedById { get; set; }

        [Display(Name = "Industry", Prompt = "Industry")]
        public int? IndustryId { get; set; } = 0;

        [Display(Name = "Employer/Business Name(if self Employed)", Prompt = "Name (if self Employed)")]
        public string? BusinessName { get; set; }

        [Display(Name = "Occupation", Prompt = "Occupation")]
        public string? OccupationStatus { get; set; }

        [Display(Name = "Years in Employment/Business", Prompt = "Years")]
        public int? YearsInEmployment { get; set; }

        [Display(Name = "Position & Department", Prompt = "Position & Department")]
        public string? EmploymentPosition { get; set; }

        [Display(Name = "Business Tel No", Prompt = "Tel No")]
        public string? BusinessTelNo { get; set; }
    }
}