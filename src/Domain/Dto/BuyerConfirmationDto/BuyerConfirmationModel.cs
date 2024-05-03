using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.BuyerConfirmationDto
{
    public class BuyerConfirmationModel
    {
        public int Id { get; set; }

        public int ApplicantsPersonalInformationId { get; set; }

        [Display(Name = "Pag-IBIG MID Number/RTN", Prompt = "XXXX-XXXX-XXXX")]
        public string? PagibigNumber { get; set; }

        [Display(Name = "Name Of Project Proponent")]
        public string? ProjectProponentName { get; set; }

        [Display(Name = "Juridical Personality")]
        public int? JuridicalPersonalityId { get; set; }

        [Display(Name = "Last Name", Prompt = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "First Name", Prompt = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name", Prompt = "Middle Name")]
        public string? MiddleName { get; set; }

        public string Name
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }

        [Display(Name = "Extension Name", Prompt = "Name Extension")]
        public string? Suffix { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth", Prompt = "Birth Date", Description = "(mm/dd/yyyy)")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Mothers Maiden Name")]
        public string? MothersMaidenName { get; set; }

        [DisplayName("Marital Status")]
        public string MaritalStatus { get; set; }

        [DisplayName("Employment Status")]
        public string? OccupationStatus { get; set; }

        //Present Address

        [Display(Name = "Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? PresentUnitName { get; set; }

        [Display(Name = "Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? PresentBuildingName { get; set; }

        [Display(Name = "Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? PresentLotName { get; set; }

        [Display(Name = "Street Name", Prompt = "Street Name")]
        public string? PresentStreetName { get; set; }

        [Display(Name = "Subdivision", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? PresentSubdivisionName { get; set; }

        [Required]
        [Display(Name = "Barangay", Prompt = "Barangay Name", Description = "Barangay")]
        public string? PresentBaranggayName { get; set; }

        [Required]
        [Display(Name = "Municipality/City", Prompt = "Municipality/City Name", Description = "Municipality/City")]
        public string? PresentMunicipalityName { get; set; }

        [Required]
        [Display(Name = "Province and State Country (if abroad)", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? PresentProvinceName { get; set; }

        [Required]
        [Display(Name = "ZIP Code", Prompt = "Zip Code")]
        public string? PresentZipCode { get; set; }

        [Display(Name = "Home Number", Prompt = "Home Number")]
        public string? HomeNumber { get; set; }

        [Required]
        [Display(Name = "Cell Phone", Prompt = "Mobile Number", Description = "Cell Phone / Mobile Number")]
        public string? MobileNumber { get; set; }

        [Required]
        [Display(Name = "Business", Prompt = "indicate local,if any", Description = "Cell Phone / Mobile Number")]
        public string? BusinessTelNo { get; set; }

        [Required]
        [Display(Name = "Personal Email Address", Prompt = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Company/Employer/Business Name", Prompt = "Name")]
        public string? CompanyEmployerName { get; set; }

        [Display(Name = "Company/Employer/Business Address", Prompt = "Address")]
        public string? CompanyEmployerAddress { get; set; }

        //Spouse

        [Display(Name = "Last Name", Prompt = "Last Name")]
        public string? SpouseLastName { get; set; }

        [Display(Name = "First Name", Prompt = "First Name")]
        public string? SpouseFirstName { get; set; }

        [Display(Name = "Middle Name", Prompt = "Middle Name")]
        public string? SpouseMiddleName { get; set; }

        [Display(Name = "Extension Name", Prompt = "Extension Name")]
        public string? SpouseSuffix { get; set; }

        [Display(Name = "Company/Employer/Business Name", Prompt = "Name")]
        public string? SpouseCompanyEmployerName { get; set; }

        [Display(Name = "Company/Employer/Business Address", Prompt = "Address")]
        public string? SpouseCompanyEmployerAddress { get; set; }

        [Display(Name = "Monthly Salary", Prompt = "(Basic + Allowances)")]
        public decimal? MonthlySalary { get; set; }

        [Display(Name = "Spouse Monthly Salary", Prompt = "(Basic + Allowance)")]
        public decimal? SpouseMonthlySalary { get; set; }

        [Display(Name = "Do you have other sources of income aside from salary?")]
        public decimal isOtherSourceofIncome { get; set; }

        [Display(Name = "Source of Additional Income")]
        public decimal AdditionalSourceIncome { get; set; }

        [Display(Name = "Average Monthly Additional Income")]
        public decimal AverageMonthlyAdditionalIncome { get; set; }

        [Display(Name = "Afford Monthly Amortization", Prompt = "If you will be granted a Pag-IBIG Housing Loan, how much can you afford to pay as your monthly amortization?")]
        public decimal AffordMonthlyAmortization { get; set; }

        [Display(Name = "Are you a Pag-IBIG member?")]
        public bool isPagibigMember { get; set; }

        [Display(Name = "Have you availed a Pag-IBIG Housing Loan?")]
        public bool isPagibigAvailedLoan { get; set; }

        [Display(Name = "Have you been a co-borrower of a Pag-IBIG housing loan?")]
        public bool isPagibigCoBorrower { get; set; }

        [Display(Name = "Do you wish to pursue your housing loan application with the Project Proponent?")]
        public bool ispursueProjectProponent { get; set; }

        [Display(Name = "Have you been informed of the terms and conditions of your loan")]
        public bool isInformedTermsConditions { get; set; }

        [Display(Name = "House/Unit Model")]
        public string? HouseUnitModel { get; set; }

        [Display(Name = "Monthly")]
        public decimal? SellingPrice { get; set; }

        [Display(Name = "Monthly Amortization")]
        public decimal? MonthlyAmortization { get; set; }
    }
}