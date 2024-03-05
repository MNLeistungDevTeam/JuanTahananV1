using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApplicantsDto
{
    public class PurposeOfLoanSelectedModel
    {
        public int Id { get; set; }
        [Display(Name = "Purpose of loan", Prompt = "Select Loans")]
        public int PurposeOfLoanId { get; set; }
        public int LoanParticularsInformationId { get; set; }
        [Required(ErrorMessage = "this field is required!")]
        [Display(Name = "Purpose of loan", Prompt = "Select Loans")]
        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }
    }
}
