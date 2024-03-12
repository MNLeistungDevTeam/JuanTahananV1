using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DMS.Domain.Dto.BasicBeneficiaryDto

{
    public class BasicBeneficiaryInformationModel
    {
        #region ApplicantPersonalInformation

        [Required]
        [DisplayName("Pagibig Mid Number")]
        public string? PagibigMidNumber { get; set; }

        #endregion ApplicantPersonalInformation

        #region Barrowers

        [Required]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [Required]
        [DisplayName("Middle")]
        public string? MiddleName { get; set; }

        [Required]
        [DisplayName("Gender")]
        public string? Gender { get; set; }

        [Required]
        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [Required]
        [DisplayName("Mobile Number")]
        public string? MobileNumber { get; set; }

        // Addresses
        [Required]
        [DisplayName("Unit")]
        public string? PresentUnitName { get; set; }

        [Required]
        [DisplayName("Building")]
        public string? PresentBuildingName { get; set; }

        [Required]
        [DisplayName("Lot")]
        public string? PresentLotName { get; set; }

        [Required]
        [DisplayName("Street")]
        public string? PresentStreetName { get; set; }

        [Required]
        [DisplayName("Subdivision")]
        public string? PresentSubdivisionName { get; set; }

        [Required]
        [DisplayName("Baranggay")]
        public string? PresentBaranggayName { get; set; }

        [Required]
        [DisplayName("Municipality")]
        public string? PresentMunicipalityName { get; set; }

        [Required]
        [DisplayName("Province")]
        public string? PresentProvinceName { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        public string? PresentZipCode { get; set; }

        [Required]
        [DisplayName("Unit")]
        public string? PermanentUnitName { get; set; }

        [Required]
        [DisplayName("Building")]
        public string? PermanentBuildingName { get; set; }

        [Required]
        [DisplayName("Lot")]
        public string? PermanentLotName { get; set; }

        [Required]
        [DisplayName("Street")]
        public string? PermanentStreetName { get; set; }

        [Required]
        [DisplayName("Subdivision")]
        public string? PermanentSubdivisionName { get; set; }

        [Required]
        [DisplayName("Baranggay")]
        public string? PermanentBaranggayName { get; set; }

        [Required]
        [DisplayName("Municipality")]
        public string? PermanentMunicipalityName { get; set; }

        [Required]
        [DisplayName("Province")]
        public string? PermanentProvinceName { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        public string? PermanentZipCode { get; set; }

        #endregion Barrowers
    }
}