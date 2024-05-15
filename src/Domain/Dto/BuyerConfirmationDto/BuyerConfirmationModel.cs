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

        public int? ApplicantsPersonalInformationId { get; set; }
        public int? UserId { get; set; }

        [Display(Name = "Pag-IBIG MID Number/RTN", Prompt = "XXXX-XXXX-XXXX")]
        public string? PagibigNumber { get; set; }

        public string? Code { get; set; }

        [Display(Name = "Name of Project Proponent", Prompt = "Name of Project Proponent")]
        public string? ProjectProponentName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Select Juridical Personality")]
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

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth", Prompt = "Birth Date", Description = "(mm/dd/yyyy)")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Mothers Maiden Name", Prompt = "Mothers Maiden Name")]
        public string? MothersMaidenName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Marital Status")]
        public string? MaritalStatus { get; set; }

        [Required(ErrorMessage = "This field is required")]
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

        [Display(Name = "Business", Prompt = "indicate local,if any", Description = "Cell Phone / Mobile Number")]
        public string? BusinessTelNo { get; set; }

        [Required]
        [Display(Name = "Personal Email Address", Prompt = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Company/Employer/Business Name", Prompt = "Name")]
        public string? CompanyEmployerName { get; set; }

        //CompanyAddress

        [Display(Name = "Company Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? CompanyUnitName { get; set; }

        [Display(Name = "Company Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? CompanyBuildingName { get; set; }

        [Display(Name = "Company Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? CompanyLotName { get; set; }

        [Display(Name = "Company Street Name", Prompt = "Street Name")]
        public string? CompanyStreetName { get; set; }

        [Display(Name = "Company Subdivision Name", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? CompanySubdivisionName { get; set; }

        [Display(Name = "Company Barangay Name", Prompt = "Barangay Name", Description = "Barangay")]
        public string? CompanyBaranggayName { get; set; }

        [Display(Name = "Company Municipality/City Name", Prompt = "Municipality/City Name", Description = "Municipality/City")]
        public string? CompanyMunicipalityName { get; set; }

        [Display(Name = "Company Province Name", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? CompanyProvinceName { get; set; }

        [Display(Name = "Company ZIP Code", Prompt = "Zip Code")]
        public string? CompanyZipCode { get; set; }

        //Spouse

        [Display(Name = "Spouse Last Name", Prompt = "Last Name")]
        public string? SpouseLastName { get; set; }

        [Display(Name = "Spouse First Name", Prompt = "First Name")]
        public string? SpouseFirstName { get; set; }

        [Display(Name = "Spouse Middle Name", Prompt = "Middle Name")]
        public string? SpouseMiddleName { get; set; }

        [Display(Name = "Spouse Extension Name", Prompt = "Extension Name")]
        public string? SpouseSuffix { get; set; }

        [Display(Name = "Spouse Company/Employer/Business Name", Prompt = "Name")]
        public string? SpouseCompanyEmployerName { get; set; }

        [Display(Name = "Spouse Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? SpouseCompanyUnitName { get; set; }

        [Display(Name = "Spouse Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? SpouseCompanyBuildingName { get; set; }

        [Display(Name = "Spouse Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? SpouseCompanyLotName { get; set; }

        [Display(Name = "Spouse SpouseStreet Name", Prompt = "Street Name")]
        public string? SpouseCompanyStreetName { get; set; }

        [Display(Name = "Spouse Subdivision", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? SpouseCompanySubdivisionName { get; set; }

        [Display(Name = "Spouse Barangay", Prompt = "Barangay Name", Description = "Barangay")]
        public string? SpouseCompanyBaranggayName { get; set; }

        [Display(Name = "Spouse Municipality/City", Prompt = "Municipality/City Name", Description = "Municipality/City")]
        public string? SpouseCompanyMunicipalityName { get; set; }

        [Display(Name = "Spouse Province and State Country (if abroad)", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? SpouseCompanyProvinceName { get; set; }

        [Display(Name = "Spouse ZIP Code", Prompt = "Zip Code")]
        public string? SpouseCompanyZipCode { get; set; }

        [Display(Name = "Monthly Salary", Prompt = "(Basic + Allowances)")]
        public decimal? MonthlySalary { get; set; }

        [Display(Name = "Spouse Monthly Salary", Prompt = "(Basic + Allowance)")]
        public decimal? SpouseMonthlySalary { get; set; }

        [Display(Name = "Do you have other sources of income aside from salary?")]
        public bool IsOtherSourceOfIncome { get; set; }

        [Display(Name = "Source of Additional Income", Prompt = "Source of Additional Income")]
        public string? AdditionalSourceIncome { get; set; }

        [Display(Name = "Average Monthly Additional Income")]
        public decimal AverageMonthlyAdditionalIncome { get; set; }

        [Display(Name = "Are you a Pag-IBIG member?")]
        public bool IsPagibigMember { get; set; }

        [Display(Name = "Have you availed a Pag-IBIG Housing Loan?")]
        public bool IsPagibigAvailedLoan { get; set; }

        [Display(Name = "Have you been a co-borrower of a Pag-IBIG housing loan?")]
        public bool IsPagibigCoBorrower { get; set; }

        [Display(Name = "Do you wish to pursue your housing loan application with the Project Proponent?")]
        public bool IsPursueProjectProponent { get; set; }

        [Display(Name = "Have you been informed of the terms and conditions of your loan")]
        public bool IsInformedTermsConditions { get; set; }

        [Display(Name = "House/Unit Model", Prompt = "House/Unit Model")]
        public string? HouseUnitModel { get; set; }

        [Display(Name = "Selling Price", Prompt = "0.00")]
        public decimal? SellingPrice { get; set; }

        [Display(Name = "Monthly Amortization", Prompt = "0.00")]
        public decimal? MonthlyAmortization { get; set; }

        public int? ApprovalStatus { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        [Display(Name = "Afford Monthly Amortization", Prompt = "If you will be granted a Pag-IBIG Housing Loan, how much can you afford to pay as your monthly amortization?")]
        public decimal? AffordMonthlyAmortization { get; set; }

        #region Display Prop

        public string? ApplicantFullName { get; set; }
        public string? ApplicantFirstName { get; set; }
        public string? ApplicantEmail { get; set; }
        public string? Remarks { get; set; }
        public string? ApproverRole { get; set; }

        public string? ApplicationStatus { get; set; }

        public int? SenderId { get; set; }

        #endregion Display Prop

        public int? CompanyId { get; set; }
        public int? BuyerConfirmationDocumentId { get; set; }
        public int? BuyerConfirmationDocumentStatus { get; set; }
        public string? FileName { get; set; }
        public string? FileLocation { get; set; }

        public int PropertyDeveloperId { get; set; }
        public int PropertyProjectId { get; set; }
        public int PropertyUnitId { get; set; }
        public int PropertyLocationId { get; set; }
    }
}