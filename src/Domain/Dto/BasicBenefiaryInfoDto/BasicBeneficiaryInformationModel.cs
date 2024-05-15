using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DMS.Domain.Dto.BasicBeneficiaryDto

{
    public class BasicBeneficiaryInformationModel
    {
        #region ApplicantPersonalInformation

        [Required]
        [DisplayName("Pagibig Mid Number")]
        public string? PagibigMidNumber { get; set; } = string.Empty;

        #endregion ApplicantPersonalInformation

        #region Barrowers

        [Required]
        [DisplayName("Last Name")]
        public string? LastName { get; set; } = string.Empty;

        [Required]
        [DisplayName("First Name")]
        public string? FirstName { get; set; } = string.Empty;

        [DisplayName("Middle")]
        public string? MiddleName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Gender")]
        public string? Gender { get; set; } = string.Empty;

        [DisplayName("Age")]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth", Prompt = "Birth Date", Description = "(mm/dd/yyyy)")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Birth Place")]
        public string? BirthPlace { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email Address", Prompt = "Email")]
        public string? Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Cell Phone", Prompt = "Mobile Number", Description = "Cell Phone / Mobile Number")]
        public string? MobileNumber { get; set; } = string.Empty;

        // Addresses

        [Display(Name = "Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? PresentUnitName { get; set; } = string.Empty;

        [Display(Name = "Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? PresentBuildingName { get; set; } = string.Empty;

        [Display(Name = "Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? PresentLotName { get; set; } = string.Empty;

        [Display(Name = "Street Name", Prompt = "Street Name")]
        public string? PresentStreetName { get; set; } = string.Empty;

        [Display(Name = "Subdivision", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? PresentSubdivisionName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Barangay", Prompt = "Barangay Name", Description = "Barangay")]
        public string? PresentBarangayName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Municipality/City", Prompt = "Municipality/City Name", Description = "Municipality/City")]
        public string? PresentMunicipalityName { get; set; } = string.Empty;

        [Display(Name = "Province and State Country (if abroad)", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? PresentProvinceName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "ZIP Code", Prompt = "Zip Code")]
        public string? PresentZipCode { get; set; } = string.Empty;

        [Display(Name = "Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? PermanentUnitName { get; set; } = string.Empty;

        [Display(Name = "Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? PermanentBuildingName { get; set; } = string.Empty;

        [Display(Name = "Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? PermanentLotName { get; set; } = string.Empty;

        [Display(Name = "Street Name", Prompt = "Street Name")]
        public string? PermanentStreetName { get; set; } = string.Empty;

        [Display(Name = "Subdivision", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? PermanentSubdivisionName { get; set; } = string.Empty;

        [Display(Name = "Barangay", Prompt = "Barangay Name", Description = "Barangay")]
        public string? PermanentBarangayName { get; set; } = string.Empty;

        [Display(Name = "Municipality/City", Prompt = "Municipality/City Name", Description = "Municipality/City")]
        public string? PermanentMunicipalityName { get; set; } = string.Empty;

        [Display(Name = "Province and State Country (if abroad)", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? PermanentProvinceName { get; set; } = string.Empty;

        [Display(Name = "ZIP Code", Prompt = "Zip Code")]
        public string? PermanentZipCode { get; set; } = string.Empty;

        [DisplayName("Property Developer ")]
        public string? PropertyDeveloperName { get; set; } = string.Empty;

        [DisplayName("Property Location")]
        public string? PropertyLocation { get; set; } = string.Empty;

        [DisplayName("Property Unit")]
        public string? PropertyUnitLevelName { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public int PropertyDeveloperId { get; set; }
        public int PropertyLocationId { get; set; }
        public int PropertyProjectId { get; set; }

        public int PropertyUnitId { get; set; }

        #endregion Barrowers
    }
}