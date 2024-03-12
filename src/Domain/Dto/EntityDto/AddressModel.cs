using DMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.EntityDto
{
    public class AddressModel
    {
        public int Id { get; set; }
        public int ReferenceId { get; set; }

        public string? EntityCode { get; set; }

        /// <summary>
        /// Company = 1, Vendor = 2, Customer = 3, Employee = 3
        /// </summary>
        public int ReferenceType { get; set; }

        [Required(ErrorMessage = "Street Address 1 is required.")]
        [DisplayName("Street Address 1")]
        [MaxLength(500)]
        public string StreetAddress1 { get; set; } = null!;

        [MaxLength(500)]
        [DisplayName("Street Address 2")]
        public string? StreetAddress2 { get; set; }

        public string? Baranggay { get; set; }

        [DisplayName("City/Municipality")]
        public string? CityMunicipality { get; set; }

        public string? Municipality { get; set; }
        public string? Province { get; set; }

        [DisplayName("State/Province")]
        public string? StateProvince { get; set; }

        public string? Region { get; set; }

        [DisplayName("Postal Code")]
        [Required(ErrorMessage = "Postal Code is required.")]
        public int PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [DisplayName("Country")]
        public int CountryId { get; set; }

        public string? Remarks { get; set; }
        public int CompanyId { get; set; }

        [DisplayName("Disable Address")]
        public bool IsDisabled { get; set; }

        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }

        [Required(ErrorMessage = "Set as Default is required.")]
        [DisplayName("Set as Default")]
        public bool IsDefault { get; set; }

        public string? CountryName { get; set; }

        [DisplayName("Company")]
        public string? CompanyName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [DisplayName("Name")]
        [MaxLength(50)]
        public string? AddressName { get; set; }

        public List<DropDownModel>? Countries { get; set; }

        #region Display Properties

        public string? CountryCode { get; set; }

        public string? FullAddress
        {
            get
            {
                return string.Join(" ", StreetAddress1, StreetAddress2, Baranggay, CityMunicipality, StateProvince, Region, (PostalCode == 0 ? "" : PostalCode.ToString()), CountryCode);
            }
        }

        #endregion
    }
}
