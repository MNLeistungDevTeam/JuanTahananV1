using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApplicantsDto
{
    public class ApplicantsPersonalInformationModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Housing Account Number HAN, if with existing HAN", Prompt = "Input Number")]
        [Range(0, 999999999999)]
        public long HousingAccountNumber { get; set; }

        //[Display(Name = "Pag-lBIG MID Number/RTN", Prompt = "Input Number")]
        //[Range(0, 999999999999)]
        [Display(Name = "Pag-lBIG MID Number/RTN", Prompt = "Input Pagibig Number")]
        public string? PagibigNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }
        public int? CompanyId { get; set; }

        #region Display Properties

        public string? ApplicantFullName { get; set; }
        public string? PositionName { get; set; }
        public string? ApplicationStatus { get; set; }
        

        #endregion Display Properties
    }
}