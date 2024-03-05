using System;
using System.ComponentModel;
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

        [DisplayName("Is Spouse Address Abroad")]
        public bool IsSpouseAddressAbroad { get; set; }

        [DisplayName("Employment Unit Name")]
        public string? SpouseEmploymentUnitName { get; set; }

        [DisplayName("Employment Building Name")]
        public string? SpouseEmploymentBuildingName { get; set; }

        [DisplayName("Employment Lot Name")]
        public string? SpouseEmploymentLotName { get; set; }

        [DisplayName("Employment Street Name")]
        public string? SpouseEmploymentStreetName { get; set; }

        [DisplayName("Employment Subdivision Name")]
        public string? SpouseEmploymentSubdivisionName { get; set; }

        [DisplayName("Employment Baranggay Name")]
        public string? SpouseEmploymentBaranggayName { get; set; }

        [DisplayName("Employment Municipality Name")]
        public string? SpouseEmploymentMunicipalityName { get; set; }

        [DisplayName("Employment Province Name")]
        public string? SpouseEmploymentProvinceName { get; set; }

        [DisplayName("Employment Zip Code")]
        public string? SpouseEmploymentZipCode { get; set; }

        [DisplayName("Prepared Mailing Address")]
        public string? PreparedMailingAddress { get; set; }

        [DisplayName("Preferred Time To Contact")]
        public DateTime? PreferredTimeToContact { get; set; }

        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }

        [DisplayName("Suffix")]
        public string? Suffix { get; set; }

        [DisplayName("Pagibig Mid Number")]
        public int? PagibigMidNumber { get; set; }

        [DisplayName("Tin Number")]
        public int? TinNumber { get; set; }

        [DisplayName("Citizenship")]
        public string? Citizenship { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

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

        [DisplayName("Business Number")]
        public int? BusinessNumber { get; set; }

        [DisplayName("Deleted By Id")]
        public int? DeletedById { get; set; }

        [DisplayName("Industry")]
        public int? IndustryId { get; set; } = 0;
    }
}
