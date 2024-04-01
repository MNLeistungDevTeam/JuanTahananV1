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

        [Display(Name = "Suffix", Prompt = "Suffix")]
        public string? Suffix { get; set; }

        [Display(Name = "Citizenship", Prompt = "Citizenship")]
        public string? Citizenship { get; set; }

        [DataType(DataType.Date)]

        [DisplayName("Birth Date")]
        [Display(Name = "Birth Date", Prompt = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Sex")]
        public string Sex { get; set; }

        [DisplayName("Marital Status")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Home Number", Prompt = "Home Number")]
        public string? HomeNumber { get; set; }

        [Display(Name = "Mobile Number", Prompt = "Mobile Number")]
        public string? MobileNumber { get; set; }

        [Required]
        [Display(Name = "Email", Prompt = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Present Unit", Prompt = "Unit Name")]
        public string? PresentUnitName { get; set; }

        [Display(Name = "Present Building", Prompt = "Building Name")]
        public string? PresentBuildingName { get; set; }

        [Display(Name = "Present Lot", Prompt = "Lot Name")]
        public string? PresentLotName { get; set; }

        [Display(Name = "Present Street", Prompt = "Street Name")]
        public string? PresentStreetName { get; set; }

        [Display(Name = "Present Subdivision", Prompt = "Subdivision Name")]
        public string? PresentSubdivisionName { get; set; }

        [Display(Name = "Present Baranggay", Prompt = "Baranggay Name")]
        public string? PresentBaranggayName { get; set; }

        [Display(Name = "Present Municipality", Prompt = "Municipality Name")]
        public string? PresentMunicipalityName { get; set; }

        [Display(Name = "Present Province", Prompt = "Province Name")]
        public string? PresentProvinceName { get; set; }

        [Display(Name = "Present Zip Code", Prompt = "Zip Code")]
        public string? PresentZipCode { get; set; }

        [Display(Name = "Permanent Unit", Prompt = "Unit Name")]
        public string? PermanentUnitName { get; set; }

        [Display(Name = "Permanent Building", Prompt = "Building Name")]
        public string? PermanentBuildingName { get; set; }

        [Display(Name = "Permanent Lot", Prompt = "Lot Name")]
        public string? PermanentLotName { get; set; }

        [Display(Name = "Permanent Street", Prompt = "Street Name")]
        public string? PermanentStreetName { get; set; }

        [Display(Name = "Permanent Subdivision", Prompt = "Subdivision Name")]
        public string? PermanentSubdivisionName { get; set; }

        [Display(Name = "Permanent Baranggay", Prompt = "Baranggay Name")]
        public string? PermanentBaranggayName { get; set; }

        [Display(Name = "Permanent Municipality", Prompt = "Municipality Name")]
        public string? PermanentMunicipalityName { get; set; }

        [Display(Name = "Permanent Province", Prompt = "Province Name")]
        public string? PermanentProvinceName { get; set; }

        [Display(Name = "Permanent Zip Code", Prompt = "Zip Code")]
        public string? PermanentZipCode { get; set; }

        [Display(Name = "Home Ownership", Prompt = "Home Ownership")]
        public string HomeOwnerShip { get; set; }

        [Display(Name = "Monthly Rent", Prompt = "Monthly Rent")]
        public Decimal? MonthlyRent { get; set; }

        [Display(Name = "Years of Stay", Prompt = "Years")]
        public int? YearsofStay { get; set; }

        [Display(Name = "SSS Number", Prompt = "XX-XXXXXXX-XX")]
        public string? SSSNumber { get; set; }

        [Display(Name = "TIN Number", Prompt = "XXX-XXX-XXX-XXXX")]
        public string? TinNumber { get; set; }

        [DisplayName("Occupation Status")]
        public string OcupationStatus { get; set; }

        [Display(Name = "Employer", Prompt = "Employer Name")]
        public string? EmployerName { get; set; }

        [DisplayName("Industry")]
        public string IndustryName { get; set; }

        [Display(Name = "Position", Prompt = "Position")]
        public string? PositionName { get; set; }

        [Display(Name = "Department", Prompt = "Department")]
        public string? DepartmentName { get; set; }

        [Display(Name = "Years Employment", Prompt = "Years Employement")]
        public int? YearsEmployment { get; set; }

        [Display(Name = "Number of Dependents", Prompt = "No of Dependents")]
        public int? NumberOfDependent { get; set; }

        [Display(Name = "Business Unit", Prompt = "Unit Name")]
        public string? BusinessUnitName { get; set; }

        [Display(Name = "Business Building", Prompt = "Building Name")]
        public string? BusinessBuildingName { get; set; }

        [Display(Name = "Business Lot", Prompt = "Lot Name")]
        public string? BusinessLotName { get; set; }

        [Display(Name = "Business Street", Prompt = "Street Name")]
        public string? BusinessStreetName { get; set; }

        [Display(Name = "Business Subdivision", Prompt = "Subdivision Name")]
        public string? BusinessSubdivisionName { get; set; }

        [Display(Name = "Business Baranggay", Prompt = "Baranggay Name")]
        public string? BusinessBaranggayName { get; set; }

        [Display(Name = "Business Municipality", Prompt = "Municipality Name")]
        public string? BusinessMunicipalityName { get; set; }

        [Display(Name = "Business Province", Prompt = "Province Name")]
        public string? BusinessProvinceName { get; set; }

        [Display(Name = "Business Zip Code", Prompt = "Zip Code")]
        public string? BusinessZipCode { get; set; }

        [Display(Name = "Business Country", Prompt = "Country")]
        public string? BusinessCountry { get; set; }

        [Display(Name = "Business Contact Number", Prompt = "Input number")]
        public string? BusinessContactNumber { get; set; }

        [Display(Name = "Business Direct Line Number", Prompt = "Input number")]
        public int? BusinessDirectLineNumber { get; set; }

        [Display(Name = "Business Truck Line Number", Prompt = "Input number")]
        public int? BusinessTruckLineNumber { get; set; }

        [Display(Name = "Business Email", Prompt = "Email")]
        public string? BusinessEmail { get; set; }

        [Display(Name = "Preferred Mailing Address", Prompt = "Mailing Address")]
        public string PreparedMailingAddress { get; set; }

        [Display(Name = "Preferred Time To Contact", Prompt = "Preferred Time to Contact")]
        public DateTime? PreferredTimeToContact { get; set; }

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

        [DisplayName("Is Business Address on Abroad")]
        public bool IsBusinessAddressAbroad { get; set; }

        [DisplayName("Property Developer ")]
        public string? PropertyDeveloperName { get; set; }

        [DisplayName("Property Location")]
        public string? PropertyLocation { get; set; }

        [DisplayName("Property Unit")]
        public string? PropertyUnitLevelName { get; set; }
    }
}