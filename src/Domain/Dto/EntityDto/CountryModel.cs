using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.EntityDto
{
    public class CountryModel
    {
        public int Id { get; set; }
        /// <summary>
        /// ISO Code
        /// </summary>
        [Required(ErrorMessage = "Country Code is required.")]
        public string CountryCode { get; set; } = null!;
        /// <summary>
        /// ISO3 Code
        /// </summary>
        [Required(ErrorMessage = "Country Name is required.")]
        public string CountryName { get; set; } = null!;
        public string? CurrencyCode { get; set; }
        public string? FipsCode { get; set; }
        public string? IsoNumeric { get; set; }
        public string? North { get; set; }
        public string? South { get; set; }
        public string? East { get; set; }
        public string? West { get; set; }
        public string? Capital { get; set; }
        public string? ContinentName { get; set; }
        public string? Continent { get; set; }
        public string? Languages { get; set; }
        public string? IsoAlpha3 { get; set; }
        public int? GeonameId { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime DateModified { get; set; }
    }
}
