using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.CompanyDto
{
    public class CompanyModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [DisplayName("Business Style")]
        public string? BusinessStyle { get; set; }

        [DisplayName("Tel No.")]
        public string? TelNo { get; set; }

        [DisplayName("Mobile No.")]
        public string? MobileNo { get; set; }

        [DisplayName("Fax No.")]
        public string? FaxNo { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        [Url]
        public string? Website { get; set; }

        [DisplayName("TIN")]
        public string? Tin { get; set; }

        [DisplayName("Representative Name")]
        public string? RepresentativeName { get; set; }

        [DisplayName("Representative TIN")]
        public string? RepresentativeTin { get; set; }

        [DisplayName("Representative Title/Designation")]
        public string? RepresentativeDesignation { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        //[DisplayName("Posting Period")]
        //[Range(1, 28)]
        //[Required(ErrorMessage = "Posting Period is required.")]
        //public int PostingPeriod { get; set; }

        [DisplayName("Accounting Period")]
        public string? AccountingPeriod { get; set; }

        public string? AcctngPeriodFrom { get; set; }

        public string? AcctngPeriodTo { get; set; }

        //[DisplayName("Inventory Evaluation Method")]
        //[Required(ErrorMessage = "Inventory Evaluation Method is required.")]
        //public int InvEvalMethodId { get; set; }

        public int InvEvalMethodDesc { get; set; }

        //[Range(1, 6)]
        //[DisplayName("Transaction Series Length")]
        //[Required(ErrorMessage = "Transaction Series Length is required.")]
        //public int TransactionSeriesCount { get; set; }

        [DisplayName("Disable")]
        public bool IsDisabled { get; set; }

        public int CreatedById { get; set; }

        public DateTime DateCreated { get; set; }

        public int ModifiedById { get; set; }

        public DateTime DateModified { get; set; }

        public List<CompanyLogoModel>? CompanyLogos { get; set; }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public DateTime? MinTransactionDate { get; set; }

        public DateTime? MaxTransactionDate { get; set; }

        [DisplayName("Set Currency Daily")]
        public bool IsRequiredDailySetCurrency { get; set; }
    }
}
