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

        [Required]
        [DisplayName("Age")]
        public int Age { get; set; }

        [Required]
        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

   
        [DisplayName("Birth Place")]
        public string? BirthPlace { get; set; } = string.Empty;

        [Required]
        [DisplayName("Email")]
        public string? Email { get; set; } = string.Empty;

        [Required]
        [DisplayName("Mobile Number")]
        public string? MobileNumber { get; set; } = string.Empty;

        // Addresses

        [DisplayName("Unit")]
        public string? PresentUnitName { get; set; } = string.Empty;

        [DisplayName("Building")]
        public string? PresentBuildingName { get; set; } = string.Empty;

        [DisplayName("Lot")]
        public string? PresentLotName { get; set; } = string.Empty;

        [DisplayName("Street")]
        public string? PresentStreetName { get; set; } = string.Empty;

        [DisplayName("Subdivision")]
        public string? PresentSubdivisionName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Baranggay")]
        public string? PresentBaranggayName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Municipality")]
        public string? PresentMunicipalityName { get; set; } = string.Empty;


        [DisplayName("Province")]
        public string? PresentProvinceName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Zip Code")]
        public string? PresentZipCode { get; set; } = string.Empty;

        [DisplayName("Unit")]
        public string? PermanentUnitName { get; set; } = string.Empty;

        [DisplayName("Building")]
        public string? PermanentBuildingName { get; set; } = string.Empty;

        [DisplayName("Lot")]
        public string? PermanentLotName { get; set; } = string.Empty;

        [DisplayName("Street")]
        public string? PermanentStreetName { get; set; } = string.Empty;

        [DisplayName("Subdivision")]
        public string? PermanentSubdivisionName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Barangay")]
        public string? PermanentBaranggayName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Municipality")]
        public string? PermanentMunicipalityName { get; set; } = string.Empty;

        [DisplayName("Province")]
        public string? PermanentProvinceName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Zip Code")]
        public string? PermanentZipCode { get; set; } = string.Empty;

        [DisplayName("Property Developer ")]
        public string? PropertyDeveloperName { get; set; } = string.Empty;

        [DisplayName("Property Location")]
        public string? PropertyLocation { get; set; } = string.Empty;

        [DisplayName("Property Unit")]
        public string? PropertyUnitLevelName { get; set; } = string.Empty;

        #endregion Barrowers
    }
}