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

        public string Code { get; set; }

        public string Name { get; set; }

        public string CurrencyCode { get; set; }

        public string FipsCode { get; set; }

        public string IsoNumber { get; set; }

        public string North { get; set; }

        public string South { get; set; }

        public string East { get; set; }

        public string West { get; set; }

        public string Capital { get; set; }

        public string ContinentName { get; set; }

        public string Continent { get; set; }

        public string Languages { get; set; }

        public string IsoAlpha3 { get; set; }

        public int? GeoNameId { get; set; }

        public int? CreatedById { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateModified { get; set; }
    }
}