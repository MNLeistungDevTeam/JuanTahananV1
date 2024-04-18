using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DMS.Domain.Dto.ApplicantsDto
{
    public class BarrowersInformationModel
    {
        public int Id { get; set; }

        public int ApplicantsPersonalInformationId { get; set; }

        [Display(Name = "Last Name", Prompt = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "First Name", Prompt = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name", Prompt = "Middle Name")]
        public string? MiddleName { get; set; }

        public string Name
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }

        [Display(Name = "Name Extension (if applicable)", Prompt = "Name Extension")]
        public string? Suffix { get; set; }

        [Display(Name = "Citizenship", Prompt = "Citizenship")]
        public string? Citizenship { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth", Prompt = "Birth Date", Description = "(mm/dd/yyyy)")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Sex")]
        public string Sex { get; set; }

        [DisplayName("Marital Status")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Home Number", Prompt = "Home Number")]
        public string? HomeNumber { get; set; }

        [Required]
        [Display(Name = "Cell Phone", Prompt = "Mobile Number", Description = "Cell Phone / Mobile Number")]
        public string? MobileNumber { get; set; }

        [Required]
        [Display(Name = "Email Address", Prompt = "Email")]
        public string? Email { get; set; }


        // Present

        [Display(Name = "Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? PresentUnitName { get; set; }

        [Display(Name = "Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? PresentBuildingName { get; set; }

        [Display(Name = "Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? PresentLotName { get; set; }

        [Display(Name = "Street Name", Prompt = "Street Name")]
        public string? PresentStreetName { get; set; }

        [Display(Name = "Subdivision", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? PresentSubdivisionName { get; set; }

        [Required]
        [Display(Name = "Barangay", Prompt = "Barangay Name", Description = "Barangay")]
        public string? PresentBaranggayName { get; set; }

        [Required]
        [Display(Name = "Municipality/City", Prompt = "Municipality/City Name", Description = "Municipality/City")]
        public string? PresentMunicipalityName { get; set; }

        [Required]
        [Display(Name = "Province and State Country (if abroad)", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? PresentProvinceName { get; set; }

        [Required]
        [Display(Name = "ZIP Code", Prompt = "Zip Code")]
        public string? PresentZipCode { get; set; }



        // Permanent

        [Display(Name = "Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? PermanentUnitName { get; set; }

        [Display(Name = "Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? PermanentBuildingName { get; set; }

        [Display(Name = "Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? PermanentLotName { get; set; }

        [Display(Name = "Street Name", Prompt = "Street Name")]
        public string? PermanentStreetName { get; set; }

        [Display(Name = "Subdivision", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? PermanentSubdivisionName { get; set; }

        [Display(Name = "Barangay", Prompt = "Barangay Name", Description = "Barangay")]
        public string? PermanentBaranggayName { get; set; }

        [Display(Name = "Municipality/City", Prompt = "Municipality/City Name", Description = "Municipality/City")]
        public string? PermanentMunicipalityName { get; set; }

        [Display(Name = "Province and State Country (if abroad)", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? PermanentProvinceName { get; set; }

        [Display(Name = "ZIP Code", Prompt = "Zip Code")]
        public string? PermanentZipCode { get; set; }



        [Display(Name = "Home Ownership", Prompt = "Home Ownership")]
        public string HomeOwnerShip { get; set; }

        [Display(Name = "Monthly Rent", Prompt = "Monthly Rent")]
        public Decimal? MonthlyRent { get; set; }

        [Display(Name = "Years of stay in Present Home Address", Prompt = "Years", Description = "Years of stay in Present Home Address")]
        public int? YearsofStay { get; set; }

        [Display(Name = "EE SSS/GSIS ID No.", Prompt = "XX-XXXXXXX-XX", Description = "EE SSS/GSIS ID No.")]
        public string? SSSNumber { get; set; }

        [Display(Name = "TIN", Prompt = "XXX-XXX-XXX-XXXX")]
        public string? TinNumber { get; set; }

        [Required]
        [DisplayName("Occupation")]
        public string? OccupationStatus { get; set; }


        [Display(Name = "Employer/Business Name (if self-employed)", Prompt = "Employer Name", Description = "Employer/Business Name (if self-employed)")]
        public string? EmployerName { get; set; }

        [DisplayName("Industry")]
        public string? IndustryName { get; set; }

        [Display(Name = "Position and Department", Prompt = "Position")]
        public string? PositionName { get; set; }

        [Display(Name = "Department", Prompt = "Department")]
        public string? DepartmentName { get; set; }

        [Display(Name = "Years in Employment/Business", Prompt = "Years in Employment/Business")]
        public int? YearsEmployment { get; set; }

        [Display(Name = "No. of Dependent/s", Prompt = "No. of Dependent/s")]
        public int? NumberOfDependent { get; set; }

        [Display(Name = "Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? BusinessUnitName { get; set; }

        [Display(Name = "Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? BusinessBuildingName { get; set; }

        [Display(Name = "Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? BusinessLotName { get; set; }

        [Display(Name = "Street Name", Prompt = "Street Name")]
        public string? BusinessStreetName { get; set; }

        [Display(Name = "Subdivision", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? BusinessSubdivisionName { get; set; }

        [Display(Name = "Barangay", Prompt = "Baranggay Name", Description = "Barangay")]
        public string? BusinessBaranggayName { get; set; }

        [Display(Name = "Municipality/City", Prompt = "Municipality Name", Description = "Municipality/City")]
        public string? BusinessMunicipalityName { get; set; }

        [Display(Name = "Province and State Country (if abroad)", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? BusinessProvinceName { get; set; }

        [Display(Name = "ZIP Code", Prompt = "Zip Code")]
        public string? BusinessZipCode { get; set; }

        [Display(Name = "Business Country", Prompt = "Country")]
        public string? BusinessCountry { get; set; }

        [Display(Name = "Business Contact Number", Prompt = "Input number")]
        public string? BusinessContactNumber { get; set; }

        [Display(Name = "Business (Direct Line)", Prompt = "Input number")]
        public string? BusinessDirectLineNumber { get; set; }

        [Display(Name = "Business (Trunk Line)", Prompt = "Input number")]
        public string? BusinessTruckLineNumber { get; set; }

        [Display(Name = "Employer/Business Email Address", Prompt = "Email")]
        public string? BusinessEmail { get; set; }

        [Display(Name = "Preferred Mailing Address", Prompt = "Mailing Address")]
        public string PreparedMailingAddress { get; set; }

        [Display(Name = "Preferred Time to be Contacted (for Employer)", Prompt = "Preferred Time to Contact")]
        public string? PreferredTimeToContact { get; set; }

        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Created By Id")]
        public int CreatedById { get; set; }

        [DisplayName("Date Modified")]
        public DateTime? DateModified { get; set; }

        [DisplayName("Modified By Id")]
        public int? ModifiedById { get; set; }

        [DisplayName("Date Deleted")]
        public DateTime? DateDeleted { get; set; }

        [DisplayName("Deleted By Id")]
        public int? DeletedById { get; set; }

        [DisplayName("Is Permanent Address on Abroad")]
        public bool IsPermanentAddressAbroad { get; set; }

        [DisplayName("Is Present Address on Abroad")]
        public bool IsPresentAddressAbroad { get; set; }
        
        [DisplayName("My Present Address is same as my Permanent Address")]
        public bool PresentAddressIsPermanentAddress { get; set; }

        [DisplayName("Is Business Address on Abroad")]
        public bool IsBusinessAddressAbroad { get; set; }

        [DisplayName("Property Developer ")]
        public string? PropertyDeveloperName { get; set; }

        [DisplayName("Property Location")]
        public string? PropertyLocation { get; set; }

        [DisplayName("Property Unit")]
        public string? PropertyUnitLevelName { get; set; }

        public bool IsPresentAddressPermanentAddress
        { 
            get 
            { 
                return (
                    PresentBuildingName == PermanentBuildingName
                    && PresentLotName == PermanentLotName
                    && PresentSubdivisionName == PermanentSubdivisionName
                    && PresentBaranggayName == PermanentBaranggayName
                    && PresentMunicipalityName == PermanentMunicipalityName
                    && PresentProvinceName == PermanentProvinceName
                    && PresentZipCode == PermanentZipCode
                );
            } 
        }
    }
}