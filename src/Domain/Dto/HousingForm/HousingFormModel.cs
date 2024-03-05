using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.ApplicantsDto;

namespace Template.Domain.Dto.HousingForm
{
    public class HousingFormModel
    {
        public ApplicantsPersonalInformationModel? ApplicantsPersonalInformation { get; set; }
        public LoanParticularsInformationModel? LoanParticularsInformation { get; set; }
        public CollateralInformationModel? CollateralInformation { get; set; }
        public BarrowersInformationModel? BarrowersInformationModel { get; set; }
        public Form2PageModel Form2PageModel { get; set; } = new();
        public SpouseModel? SpouseModel { get; set; }
        public byte[]? form1 { get; set; }
        public byte[]? form2 { get; set; }
    }
}
