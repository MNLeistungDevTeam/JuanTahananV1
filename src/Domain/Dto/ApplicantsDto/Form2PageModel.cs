using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace DMS.Domain.Dto.ApplicantsDto
{
    public class Form2PageModel
    {
        public int Id { get; set; }

        public int ApplicantsPersonalInformationId { get; set; }

        [Display(Prompt = "Bank name")]
        public string Bank1 { get; set; }

        [Display(Prompt = "Bank name")]
        public string Bank2 { get; set; }

        [Display(Prompt = "Bank name")]
        public string Bank3 { get; set; }

        [Display(Prompt = "Branch address")]
        public string BranchAddress1 { get; set; }

        [Display(Prompt = "Branch address")]
        public string BranchAddress2 { get; set; }

        [Display(Prompt = "Branch address")]
        public string BranchAddress3 { get; set; }

        [Display(Prompt = "Type of account")]
        public string TypeOfAccount1 { get; set; }

        [Display(Prompt = "Type of account")]
        public string TypeOfAccount2 { get; set; }

        [Display(Prompt = "Type of account")]
        public string TypeOfAccount3 { get; set; }

        [Display(Prompt = "Account number")]
        public long? AccountNumber1 { get; set; }

        [Display(Prompt = "Account number")]
        public long? AccountNumber2 { get; set; }

        [Display(Prompt = "Account number")]
        public long? AccountNumber3 { get; set; }
        [DataType(DataType.Date)]

        [Display(Prompt = "Date opened")]
        public DateTime? DateOpened1 { get; set; }
        [DataType(DataType.Date)]


        [Display(Prompt = "Date opened")]
        public DateTime? DateOpened2 { get; set; }

        [DataType(DataType.Date)]

        [Display(Prompt = "Date opened")]
        public DateTime? DateOpened3 { get; set; }

        [Display(Prompt = "Average balance")]
        public decimal? AverageBalance1 { get; set; }

        [Display(Prompt = "Average balance")]
        public decimal? AverageBalance2 { get; set; }

        [Display(Prompt = "Average balance")]
        public decimal? AverageBalance3 { get; set; }

        [Display(Prompt = "Issuer name")]
        public string IssuerName1 { get; set; }

        [Display(Prompt = "Issuer name")]
        public string IssuerName2 { get; set; }

        [Display(Prompt = "Issuer name")]
        public string IssuerName3 { get; set; }

        [Display(Prompt = "Card Type")]
        public string CardType1 { get; set; }

        [Display(Prompt = "Card Type")]
        public string CardType2 { get; set; }

        [Display(Prompt = "Card Type")]
        public string CardType3 { get; set; }

        [DataType(DataType.Date)]

        [Display(Prompt = "Card expiration date")]
        public DateTime? CardExpiration1 { get; set; }
        [DataType(DataType.Date)]


        [Display(Prompt = "Card expiration date")]
        public DateTime? CardExpiration2 { get; set; }

        [DataType(DataType.Date)]

        [Display(Prompt = "Card expiration date")]
        public DateTime? CardExpiration3 { get; set; }

        [Display(Prompt = "Card limit")]
        public decimal? CreditLimit1 { get; set; }

        [Display(Prompt = "Card limit")]
        public decimal? CreditLimit2 { get; set; }

        [Display(Prompt = "Card limit")]
        public decimal? CreditLimit3 { get; set; }

        [Display(Prompt = "Location")]
        public string Location1 { get; set; }

        [Display(Prompt = "Location")]
        public string Location2 { get; set; }

        [Display(Prompt = "Location")]
        public string Location3 { get; set; }

        [Display(Prompt = "Type of property")]
        public string TypeOfProperty1 { get; set; }

        [Display(Prompt = "Type of property")]
        public string TypeOfProperty2 { get; set; }

        [Display(Prompt = "Type of property")]
        public string TypeOfProperty3 { get; set; }

        [Display(Prompt = "Aquistion cost")]
        public decimal? AquisitionCost1 { get; set; }

        [Display(Prompt = "Aquistion cost")]
        public decimal? AquisitionCost2 { get; set; }

        [Display(Prompt = "Aquistion cost")]
        public decimal? AquisitionCost3 { get; set; }

        [Display(Prompt = "Market value")]
        public decimal? MarketValue1 { get; set; }

        [Display(Prompt = "Market value")]
        public decimal? MarketValue2 { get; set; }

        [Display(Prompt = "Market value")]
        public decimal? MarketValue3 { get; set; }

        [Display(Prompt = "Mortgage balance")]
        public decimal? MortgageBalance1 { get; set; }

        [Display(Prompt = "Mortgage balance")]
        public decimal? MortgageBalance2 { get; set; }

        [Display(Prompt = "Mortgage balance")]
        public decimal? MortgageBalance3 { get; set; }

        [Display(Prompt = "Rental income")]
        public decimal? RentalIncome1 { get; set; }

        [Display(Prompt = "Rental income")]
        public decimal? RentalIncome2 { get; set; }

        [Display(Prompt = "Rental income")]
        public decimal? RentalIncome3 { get; set; }

        [Display(Prompt = "Creditor and address")]
        public string CreditorAndAddress1 { get; set; }

        [Display(Prompt = "Creditor and address")]
        public string CreditorAndAddress2 { get; set; }

        [Display(Prompt = "Creditor and address")]
        public string CreditorAndAddress3 { get; set; }

        [Display(Prompt = "Security")]
        public string Security1 { get; set; }

        [Display(Prompt = "Security")]
        public string Security2 { get; set; }

        [Display(Prompt = "Security")]
        public string Security3 { get; set; }

        [Display(Prompt = "Type")]
        public string Type1 { get; set; }

        [Display(Prompt = "Type")]
        public string Type2 { get; set; }

        [Display(Prompt = "Type")]
        public string Type3 { get; set; }

        [Display(Prompt = "Amount balance")]
        public decimal? AmountBalance1 { get; set; }

        [Display(Prompt = "Amount balance")]
        public decimal? AmountBalance2 { get; set; }

        [Display(Prompt = "Amount balance")]
        public decimal? AmountBalance3 { get; set; }
        [DataType(DataType.Date)]


        [Display(Prompt = "Maturity date")]
        public DateTime? MaturityDateTime1 { get; set; }

        [DataType(DataType.Date)]

        [Display(Prompt = "Maturity date")]
        public DateTime? MaturityDateTime2 { get; set; }
        [DataType(DataType.Date)]


        [Display(Prompt = "Maturity date")]
        public DateTime? MaturityDateTime3 { get; set; }

        [Display(Prompt = "Amortization")]
        public decimal? Amortization1 { get; set; }

        [Display(Prompt = "Amortization")]
        public decimal? Amortization2 { get; set; }

        [Display(Prompt = "Amortization")]
        public decimal? Amortization3 { get; set; }

        [DisplayName("Are there past or pending cases against you? \n if Yes, please indicate the nature, plaintiff,amount involved and the status")]
        public string PendingCase { get; set; }

        [DisplayName("Do you have past due obligations? \n if yes, please indicate the creditor's name,nature, amount involved and due date")]
        public string PastDue { get; set; }

        [DisplayName("Was your bank account ever closed because of mishandling or issuance of bouncing checks?, if yes please indicate the bank's name, nature amount and date")]
        public string BouncingChecks { get; set; }

        [DisplayName("Have you ever been diagnosed,treated or given medical advice by physician or other health care provider?, if yes please indicate the condition/diagnosis")]
        public string MedicalAdvice { get; set; }

        [Display(Prompt = "Bank financial")]
        public string BankFinancial1 { get; set; }

        [Display(Prompt = "Bank financial")]
        public string BankFinancial2 { get; set; }

        [Display(Prompt = "Bank financial")]
        public string BankFinancial3 { get; set; }

        [Display(Prompt = "Address")]
        public string Address1 { get; set; }

        [Display(Prompt = "Address")]
        public string Address2 { get; set; }

        [Display(Prompt = "Address")]
        public string Address3 { get; set; }

        [Display(Prompt = "Purpose")]
        public string Purpose1 { get; set; }

        [Display(Prompt = "Purpose")]
        public string Purpose2 { get; set; }

        [Display(Prompt = "Purpose")]
        public string Purpose3 { get; set; }

        [Display(Prompt = "Loan Security")]
        public string LoanSecurity1 { get; set; }

        [Display(Prompt = "Loan Security")]
        public string LoanSecurity2 { get; set; }

        [Display(Prompt = "Loan Security")]
        public string LoanSecurity3 { get; set; }

        [Display(Prompt = "Highest Amount")]
        public decimal? HighestAmount1 { get; set; }

        [Display(Prompt = "Highest Amount")]
        public decimal? HighestAmount2 { get; set; }

        [Display(Prompt = "Highest Amount")]
        public decimal? HighestAmount3 { get; set; }

        [Display(Prompt = "Present Balance")]
        public decimal? PresentBalance1 { get; set; }

        [Display(Prompt = "Present Balance")]
        public decimal? PresentBalance2 { get; set; }

        [Display(Prompt = "Present Balance")]
        public decimal? PresentBalance3 { get; set; }

        [DataType(DataType.Date)]

        [Display(Prompt = "Date Obtained")]
        public DateTime? DateObtained1 { get; set; }
        [DataType(DataType.Date)]


        [Display(Prompt = "Date Obtained")]
        public DateTime? DateObtained2 { get; set; }
        [DataType(DataType.Date)]


        [Display(Prompt = "Date Obtained")]
        public DateTime? DateObtained3 { get; set; }

        [DataType(DataType.Date)]

        [Display(Prompt = "Date Fully Paid")]
        public DateTime? DateFullyPaid1 { get; set; }

        [DataType(DataType.Date)]

        [Display(Prompt = "Date Fully Paid")]
        public DateTime? DateFullyPaid2 { get; set; }
        [DataType(DataType.Date)]


        [Display(Prompt = "Date Fully Paid")]
        public DateTime? DateFullyPaid3 { get; set; }

        [Display(Prompt = "Name Supplier")]
        public string NameSupplier1 { get; set; }

        [Display(Prompt = "Name Supplier")]
        public string NameSupplier2 { get; set; }

        [Display(Prompt = "Name Supplier")]
        public string NameSupplier3 { get; set; }

        [Display(Prompt = "Trade Address")]
        public string TradeAddress1 { get; set; }

        [Display(Prompt = "Trade Address")]
        public string TradeAddress2 { get; set; }

        [Display(Prompt = "Trade Address")]
        public string TradeAddress3 { get; set; }

        [Display(Prompt = "Trade Telephone No")]
        public long? TradeTellNo1 { get; set; }

        [Display(Prompt = "Trade Telephone No")]
        public long? TradeTellNo2 { get; set; }

        [Display(Prompt = "Trade Telephone No")]
        public long? TradeTellNo3 { get; set; }

        [Display(Prompt = "Character Name Supplier")]
        public string CharacterNameSupplier1 { get; set; }

        [Display(Prompt = "Character Name Supplier")]
        public string CharacterNameSupplier2 { get; set; }

        [Display(Prompt = "Character Name Supplier")]
        public string CharacterNameSupplier3 { get; set; }

        [Display(Prompt = "Character Address")]
        public string CharacterAddress1 { get; set; }

        [Display(Prompt = "Character Address")]
        public string CharacterAddress2 { get; set; }

        [Display(Prompt = "Character Address")]
        public string CharacterAddress3 { get; set; }

        [Display(Prompt = "Character Telephone No")]
        public long? CharacterTellNo1 { get; set; }

        [Display(Prompt = "Character Telephone No")]
        public long? CharacterTellNo2 { get; set; }

        [Display(Prompt = "Character Telephone No")]
        public long? CharacterTellNo3 { get; set; }

        [Display(Prompt = "First Name")]
        public string FirstName { get; set; }

        [Display(Prompt = "Middle Name")]
        public string MIddleName { get; set; }

        [Display(Prompt = "Suffix")]
        public string Suffix { get; set; }

        [Display(Prompt = "Last Name")]
        public string LastName { get; set; }

        [Display(Prompt = "Pag-IBIG MID Number")]
        public string PagibigNumber { get; set; }

        [Display(Prompt = "TIN Number")]
        public string TinNumber { get; set; }

        [Display(Prompt = "Contact Number")]
        public string ContactNumber { get; set; }

        [Display(Prompt = "Email")]
        public string Email { get; set; }

        public int? SourcePagibigFundId { get; set; }

        public DateTime? DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }

        [Display(Prompt = "Agreement")]
        public bool? Agreement { get; set; }

        [Display(Prompt = "Seller's Unit Name")]
        public string SellersUnitName { get; set; }

        [Display(Prompt = "Seller's Building Name")]
        public string SellersBuildingName { get; set; }

        [Display(Prompt = "Seller's Lot Name")]
        public string SellersLotName { get; set; }

        [Display(Prompt = "Seller's Street Name")]
        public string SellersStreetName { get; set; }

        [Display(Prompt = "Seller's Subdivision Name")]
        public string SellersSubdivisionName { get; set; }

        [Display(Prompt = "Seller's Baranggay Name")]
        public string SellersBaranggayName { get; set; }

        [Display(Prompt = "Seller's Municipality Name")]
        public string SellersMunicipalityName { get; set; }

        [Display(Prompt = "Seller's Province Name")]
        public string SellersProvinceName { get; set; }

        [Display(Prompt = "Seller's Zip Code")]
        public string SellersZipCode { get; set; }
    }
}