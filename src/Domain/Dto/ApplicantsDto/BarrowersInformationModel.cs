using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DMS.Domain.Dto.ApplicantsDto
{
    public class BarrowersInformationModel
    {
        public int Id { get; set; }

        public int? ApplicantsPersonalInformationId { get; set; } = 0;

        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [DisplayName("Middle")]
        public string? MiddleName { get; set; }

        public string Name
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }

        [DisplayName("Suffix")]
        public string? Suffix { get; set; }

        [DisplayName("Citizenship")]
        public string? Citizenship { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Sex")]
        public string? Sex { get; set; }

        [DisplayName("Marital Status")]
        public string? MaritalStatus { get; set; }

        [DisplayName("Home Number")]
        public string? HomeNumber { get; set; }

        [DisplayName("Mobile Number")]
        public string? MobileNumber { get; set; }

        [DisplayName("Email")]
        public string? Email { get; set; }

        [DisplayName("Present Unit")]
        public string? PresentUnitName { get; set; }

        [DisplayName("Present Building")]
        public string? PresentBuildingName { get; set; }

        [DisplayName("Present Lot")]
        public string? PresentLotName { get; set; }

        [DisplayName("Present Street")]
        public string? PresentStreetName { get; set; }

        [DisplayName("Present Subdivision")]
        public string? PresentSubdivisionName { get; set; }

        [DisplayName("Present Baranggay")]
        public string? PresentBaranggayName { get; set; }

        [DisplayName("Present Municipality")]
        public string? PresentMunicipalityName { get; set; }

        [DisplayName("Present Province")]
        public string? PresentProvinceName { get; set; }

        [DisplayName("Present Zip Code")]
        public string? PresentZipCode { get; set; }

        [DisplayName("Permanent Unit")]
        public string? PermanentUnitName { get; set; }

        [DisplayName("Permanent Building")]
        public string? PermanentBuildingName { get; set; }

        [DisplayName("Permanent Lot")]
        public string? PermanentLotName { get; set; }

        [DisplayName("Permanent Street")]
        public string? PermanentStreetName { get; set; }

        [DisplayName("Permanent Subdivision")]
        public string? PermanentSubdivisionName { get; set; }

        [DisplayName("Permanent Baranggay")]
        public string? PermanentBaranggayName { get; set; }

        [DisplayName("Permanent Municipality")]
        public string? PermanentMunicipalityName { get; set; }

        [DisplayName("Permanent Province")]
        public string? PermanentProvinceName { get; set; }

        [DisplayName("Permanent Zip Code")]
        public string? PermanentZipCode { get; set; }

        [DisplayName("Home Ownership")]
        public string? HomeOwnerShip { get; set; }

        [DisplayName("Years of Stay")]
        public int? YearsofStay { get; set; }

        [DisplayName("SSS Number")]
        public string? SSSNumber { get; set; }

        [DisplayName("TIN Number")]
        public string? TinNumber { get; set; }

        [DisplayName("Occupation Status")]
        public string? OcupationStatus { get; set; }

        [DisplayName("Employer")]
        public string? EmployerName { get; set; }

        [DisplayName("Industry")]
        public string? IndustryName { get; set; }

        [DisplayName("Position")]
        public string? PositionName { get; set; }

        [DisplayName("Department")]
        public string? DepartmentName { get; set; }

        [DisplayName("Years Employment")]
        public int? YearsEmployment { get; set; }

        [DisplayName("Number of Dependents")]
        public int? NumberOfDependent { get; set; }

        [DisplayName("Business Unit")]
        public string? BusinessUnitName { get; set; }

        [DisplayName("Business Building")]
        public string? BusinessBuildingName { get; set; }

        [DisplayName("Business Lot")]
        public string? BusinessLotName { get; set; }

        [DisplayName("Business Street")]
        public string? BusinessStreetName { get; set; }

        [DisplayName("Business Subdivision")]
        public string? BusinessSubdivisionName { get; set; }

        [DisplayName("Business Baranggay")]
        public string? BusinessBaranggayName { get; set; }

        [DisplayName("Business Municipality")]
        public string? BusinessMunicipalityName { get; set; }

        [DisplayName("Business Province")]
        public string? BusinessProvinceName { get; set; }

        [DisplayName("Business Zip Code")]
        public string? BusinessZipCode { get; set; }

        [DisplayName("Business Country")]
        public string? BusinessCountry { get; set; }

        [DisplayName("Business Contact Number")]
        public string? BusinessContactNumber { get; set; }

        [DisplayName("Business Direct Line Number")]
        public int? BusinessDirectLineNumber { get; set; }

        [DisplayName("Business Truck Line Number")]
        public int? BusinessTruckLineNumber { get; set; }

        [DisplayName("Business Email")]
        public string? BusinessEmail { get; set; }

        [DisplayName("Prepared Mailing Address")]
        public string? PreparedMailingAddress { get; set; }

        [DisplayName("Preferred Time To Contact")]
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
    }
}