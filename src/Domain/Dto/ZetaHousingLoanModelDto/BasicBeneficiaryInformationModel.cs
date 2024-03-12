using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ZetaHousingLoanModelDto
{
    public class BasicBeneficiaryInformationModel
    {
        public int Id { get; set; }

        [DisplayName("Pagibig Mid Number")]
        public string? PagibigMidNumber { get; set; }

        #region Barrowers

        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [DisplayName("Middle")]
        public string? MiddleName { get; set; }

        [DisplayName("Gender")]
        public string? Gender { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Email")]
        public string? Email { get; set; }

        [DisplayName("Mobile Number")]
        public string? MobileNumber { get; set; }

        [DisplayName("Unit")]
        public string? PresentUnitName { get; set; }

        [DisplayName("Building")]
        public string? PresentBuildingName { get; set; }

        [DisplayName("Lot")]
        public string? PresentLotName { get; set; }

        [DisplayName("Street")]
        public string? PresentStreetName { get; set; }

        [DisplayName("Subdivision")]
        public string? PresentSubdivisionName { get; set; }

        [DisplayName("Baranggay")]
        public string? PresentBaranggayName { get; set; }

        [DisplayName("Municipality")]
        public string? PresentMunicipalityName { get; set; }

        [DisplayName("Province")]
        public string? PresentProvinceName { get; set; }

        [DisplayName("Zip Code")]
        public string? PresentZipCode { get; set; }

        [DisplayName("Unit")]
        public string? PermanentUnitName { get; set; }

        [DisplayName("Building")]
        public string? PermanentBuildingName { get; set; }

        [DisplayName("Lot")]
        public string? PermanentLotName { get; set; }

        [DisplayName("Street")]
        public string? PermanentStreetName { get; set; }

        [DisplayName("Subdivision")]
        public string? PermanentSubdivisionName { get; set; }

        [DisplayName("Baranggay")]
        public string? PermanentBaranggayName { get; set; }

        [DisplayName("Municipality")]
        public string? PermanentMunicipalityName { get; set; }

        [DisplayName("Province")]
        public string? PermanentProvinceName { get; set; }

        [DisplayName("Zip Code")]
        public string? PermanentZipCode { get; set; }

        #endregion Barrowers
    }
}