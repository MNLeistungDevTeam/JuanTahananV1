using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApplicantsDto;

public class ApplicationInfoModel
{
    public decimal TotalApplication { get; set; }
    public decimal ApplicationDraft { get; set; }
    public decimal Submitted { get; set; }
    public decimal DeveloperVerified { get; set; }
    public decimal PagIbigVerified { get; set; }
    public decimal Withdrawn { get; set; }
}