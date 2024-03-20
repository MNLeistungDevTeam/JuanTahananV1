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

        public string Bank1 { get; set; }

        public string Bank2 { get; set; }

        public string Bank3 { get; set; }

        public string BranchAddress1 { get; set; }

        public string BranchAddress2 { get; set; }

        public string BranchAddress3 { get; set; }

        public string TypeOfAccount1 { get; set; }

        public string TypeOfAccount2 { get; set; }

        public string TypeOfAccount3 { get; set; }

        public long? AccountNumber1 { get; set; }

        public long? AccountNumber2 { get; set; }

        public long? AccountNumber3 { get; set; }

        public DateTime? DateOpened1 { get; set; }

        public DateTime? DateOpened2 { get; set; }

        public DateTime? DateOpened3 { get; set; }

        public decimal? AverageBalance1 { get; set; }

        public decimal? AverageBalance2 { get; set; }

        public decimal? AverageBalance3 { get; set; }

        public string IssuerName1 { get; set; }

        public string IssuerName2 { get; set; }

        public string IssuerName3 { get; set; }

        public string CardType1 { get; set; }

        public string CardType2 { get; set; }

        public string CardType3 { get; set; }

        public DateTime? CardExpiration1 { get; set; }

        public DateTime? CardExpiration2 { get; set; }

        public DateTime? CardExpiration3 { get; set; }

        public decimal? CreditLimit1 { get; set; }

        public decimal? CreditLimit2 { get; set; }

        public decimal? CreditLimit3 { get; set; }

        public string Location1 { get; set; }

        public string Location2 { get; set; }

        public string Location3 { get; set; }

        public string TypeOfProperty1 { get; set; }

        public string TypeOfProperty2 { get; set; }

        public string TypeOfProperty3 { get; set; }

        public decimal? AquisitionCost1 { get; set; }

        public decimal? AquisitionCost2 { get; set; }

        public decimal? AquisitionCost3 { get; set; }

        public decimal? MarketValue1 { get; set; }

        public decimal? MarketValue2 { get; set; }

        public decimal? MarketValue3 { get; set; }

        public decimal? MortgageBalance1 { get; set; }

        public decimal? MortgageBalance2 { get; set; }

        public decimal? MortgageBalance3 { get; set; }

        public decimal? RentalIncome1 { get; set; }

        public decimal? RentalIncome2 { get; set; }

        public decimal? RentalIncome3 { get; set; }

        public string CreditorAndAddress1 { get; set; }

        public string CreditorAndAddress2 { get; set; }

        public string CreditorAndAddress3 { get; set; }

        public string Security1 { get; set; }

        public string Security2 { get; set; }

        public string Security3 { get; set; }

        public string Type1 { get; set; }

        public string Type2 { get; set; }

        public string Type3 { get; set; }

        public decimal? AmountBalance1 { get; set; }

        public decimal? AmountBalance2 { get; set; }

        public decimal? AmountBalance3 { get; set; }

        public DateTime? MaturityDateTime1 { get; set; }

        public DateTime? MaturityDateTime2 { get; set; }

        public DateTime? MaturityDateTime3 { get; set; }

        public decimal? Amortization1 { get; set; }

        public decimal? Amortization2 { get; set; }

        public decimal? Amortization3 { get; set; }

        [DisplayName("Are there past or pending cases against you? \n if Yes, please indicate the nature, plaintiff,amount involved and the status")]
        public string PendingCase { get; set; }

        [DisplayName("Do you have past due obligations? \n if yes, please indicate the creditor's name,nature, amount involved and due date")]
        public string PastDue { get; set; }

        [DisplayName("Was your bank account ever closed because of mishandling or issuance of bouncing checks?, if yes please indicate the bank's name, nature amount and date")]
        public string BouncingChecks { get; set; }

        [DisplayName("Have you ever been diagnosed,treated or given medical advice by physician or other health care provider?, if yes please indicate the condition/diagnosis")]
        public string MedicalAdvice { get; set; }

        public string BankFinancial1 { get; set; }

        public string BankFinancial2 { get; set; }

        public string BankFinancial3 { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Purpose1 { get; set; }

        public string Purpose2 { get; set; }

        public string Purpose3 { get; set; }

        public string LoanSecurity1 { get; set; }

        public string LoanSecurity2 { get; set; }

        public string LoanSecurity3 { get; set; }

        public decimal? HighestAmount1 { get; set; }

        public decimal? HighestAmount2 { get; set; }

        public decimal? HighestAmount3 { get; set; }

        public decimal? PresentBalance1 { get; set; }

        public decimal? PresentBalance2 { get; set; }

        public decimal? PresentBalance3 { get; set; }

        public DateTime? DateObtained1 { get; set; }

        public DateTime? DateObtained2 { get; set; }

        public DateTime? DateObtained3 { get; set; }

        public DateTime? DateFullyPaid1 { get; set; }

        public DateTime? DateFullyPaid2 { get; set; }

        public DateTime? DateFullyPaid3 { get; set; }

        public string NameSupplier1 { get; set; }

        public string NameSupplier2 { get; set; }

        public string NameSupplier3 { get; set; }

        public string TradeAddress1 { get; set; }

        public string TradeAddress2 { get; set; }

        public string TradeAddress3 { get; set; }

        public long? TradeTellNo1 { get; set; }

        public long? TradeTellNo2 { get; set; }

        public long? TradeTellNo3 { get; set; }

        public string CharacterNameSupplier1 { get; set; }

        public string CharacterNameSupplier2 { get; set; }

        public string CharacterNameSupplier3 { get; set; }

        public string CharacterAddress1 { get; set; }

        public string CharacterAddress2 { get; set; }

        public string CharacterAddress3 { get; set; }

        public long? CharacterTellNo1 { get; set; }

        public long? CharacterTellNo2 { get; set; }

        public long? CharacterTellNo3 { get; set; }

        public string FirstName { get; set; }

        public string MIddleName { get; set; }

        public string Suffix { get; set; }

        public string LastName { get; set; }

        public string PagibigNumber { get; set; }

        public string TinNumber { get; set; }

        public string ContactNumber { get; set; }

        public string Email { get; set; }

        public int? SourcePagibigFundId { get; set; }

        public DateTime? DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }

        public bool? Agreement { get; set; }

        public string SellersUnitName { get; set; }

        public string SellersBuildingName { get; set; }

        public string SellersLotName { get; set; }

        public string SellersStreetName { get; set; }

        public string SellersSubdivisionName { get; set; }

        public string SellersBaranggayName { get; set; }

        public string SellersMunicipalityName { get; set; }

        public string SellersProvinceName { get; set; }

        public string SellersZipCode { get; set; }
    }
}