using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApplicantsDto
{
    public class CollateralInformationModel
    {
        public int Id { get; set; }

        public int? ApplicantsPersonalInformationId { get; set; }

        [Display(Prompt = "Province Name")]
        public string? Province { get; set; }

        [Display(Prompt = "Municipality Name")]
        public string? Municipality { get; set; }

        [Display(Prompt = "Street Name")]
        public string? Street { get; set; }

        [Display(Name = "Name of Developer / Registered Title Holder", Prompt = "Developer Name")]
        public string? DeveloperName { get; set; }

        [Display(Name = "Property Type", Prompt = "Select Property Type")]
        public int? PropertyTypeId { get; set; }

        [Display(Name = "TCT/OCT/CCT Number", Prompt = "Input number")]
        public int? TctOctCctNumber { get; set; }

        [Display(Name = "Tax Declaration Number", Prompt = "Input number")]
        public int? TaxDeclrationNumber { get; set; }

        [Display(Name = "Lot Unit Number", Prompt = "Input number")]
        public int? LotUnitNumber { get; set; }

        [Display(Name = "Block Building Number", Prompt = "Input number")]
        public int? BlockBuildingNumber { get; set; }

        [Display(Name = "Is the property presently mortgaged?")]
        public bool IsMortgage { get; set; }

        [Display(Name = "Is the property an offsite collateral?")]
        public bool ExistingReasonChecker
        {
            get
            {
                return !string.IsNullOrEmpty(CollateralReason);
            }
        }

        [Display(Name = "Reason for use of offsite collateral", Prompt = "Input reason")]
        public string? CollateralReason { get; set; }

        [Display(Name = "Land Area / Floor Area", Prompt = "Input sqm")]
        public decimal? LandArea { get; set; }

        [Display(Name = "Age of House (for purchase of residential unit)", Prompt = "Input Number")]
        public int? HouseAge { get; set; }

        [Display(Name = "Existing Number of Storey", Prompt = "Input Number")]
        public int? NumberOfStoreys { get; set; }

        [Display(Name = "Proposed Number of Storey", Prompt = "Input Number")]
        public int? ProposedNoOfStoreys { get; set; }

        [Display(Name = "Existing Total Floor Area", Prompt = "Input sqm")]
        public decimal? ExistingTotalFloorArea { get; set; }

        [Display(Name = "Proposed Total Floor Area", Prompt = "Input sqm")]
        public decimal? ProposedTotalFloorArea { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }

        [Display(Name = "Pagibig MID Number/RTN", Prompt = "Input MID Number")]
        public int? PagibigMidNumber { get; set; }

        [Display(Name = "Housing Account Number", Prompt = "Input HAN Number")]
        public int? HanNumber { get; set; }
    }
}