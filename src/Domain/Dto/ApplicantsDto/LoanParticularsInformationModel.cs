using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.ValidationsHelper;

namespace DMS.Domain.Dto.ApplicantsDto
{
    [ConditionalTextRequired("ExistingChecker", "ExistingHousingApplicationNumber", "Application is required when Yes is checked.")]
    public class LoanParticularsInformationModel
    {
        public int Id { get; set; }

        [Display(Name = "Purpose of Loan", Prompt = "Select Purpose of Loan")]
        public int PurposeOfLoanId { get; set; }

        public int ApplicantsPersonalInformationId { get; set; }

        [Required(ErrorMessage = "this field is required")]
        [Display(Name = "Desired Re-Pricing Pediod (Years)", Prompt = "Select Re-pracing Period")]
        public int RepricingPeriod { get; set; }

        [Display(Name = "Loan Term Years", Prompt = "Input Year")]
        [Range(0, 30, ErrorMessage = "Loan Term Years must between 1 - 30")]
        public int DesiredLoanTermYears { get; set; }

        [Display(Name = "Mode of Payment (MOP)", Prompt = "Select mode of payment")]
        public int ModeOfPaymentId { get; set; }

        [DisplayName("With Existing Housing Application")]
        public bool ExistingChecker
        {
            get
            {
                return !string.IsNullOrEmpty(ExistingHousingApplicationNumber);
            }
        }

        [Display(Name = "Desired Loan Amount", Prompt = "Input desired loan amount")]
        public decimal DesiredLoanAmount { get; set; }

        [StringLength(14)]
        [Display(Name = "If yes,Indicate Housing Application No.", Prompt = "Input existing application no.")]
        public string? ExistingHousingApplicationNumber { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }
    }
}