﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DMS.Domain.Entities;

public partial class BarrowersInformation
{
    public int Id { get; set; }

    public int ApplicantsPersonalInformationId { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string Suffix { get; set; }

    public string Citizenship { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Sex { get; set; }

    public string MaritalStatus { get; set; }

    public int? HomeNumber { get; set; }

    public int? MobileNumber { get; set; }

    public int? PagibigMidNumber { get; set; }

    public int? HanNumber { get; set; }

    public string Email { get; set; }

    public bool IsPermanentAddressAbroad { get; set; }

    public bool IsPresentAddressAbroad { get; set; }

    public bool IsBusinessAddressAbroad { get; set; }

    public string PresentUnitName { get; set; }

    public string PresentBuildingName { get; set; }

    public string PresentLotName { get; set; }

    public string PresentStreetName { get; set; }

    public string PresentSubdivisionName { get; set; }

    public string PresentBaranggayName { get; set; }

    public string PresentMunicipalityName { get; set; }

    public string PresentProvinceName { get; set; }

    public string PresentZipCode { get; set; }

    public string PermanentUnitName { get; set; }

    public string PermanentBuildingName { get; set; }

    public string PermanentLotName { get; set; }

    public string PermanentStreetName { get; set; }

    public string PermanentSubdivisionName { get; set; }

    public string PermanentBaranggayName { get; set; }

    public string PermanentMunicipalityName { get; set; }

    public string PermanentProvinceName { get; set; }

    public string PermanentZipCode { get; set; }

    public string HomeOwnerShip { get; set; }

    public int? YearsofStay { get; set; }

    public int? SSSNumber { get; set; }

    public int? TinNumber { get; set; }

    public string OcupationStatus { get; set; }

    public string EmployerName { get; set; }

    public string IndustryName { get; set; }

    public string PositionName { get; set; }

    public string DepartmentName { get; set; }

    public int? YearsEmployment { get; set; }

    public int? NumberOfDependent { get; set; }

    public string BusinessUnitName { get; set; }

    public string BusinessBuildingName { get; set; }

    public string BusinessLotName { get; set; }

    public string BusinessStreetName { get; set; }

    public string BusinessSubdivisionName { get; set; }

    public string BusinessBaranggayName { get; set; }

    public string BusinessMunicipalityName { get; set; }

    public string BusinessProvinceName { get; set; }

    public string BusinessZipCode { get; set; }

    public string BusinessCountry { get; set; }

    public string BusinessContactNumber { get; set; }

    public int? BusinessDirectLineNumber { get; set; }

    public int? BusinessTruckLineNumber { get; set; }

    public string BusinessEmail { get; set; }

    public string PreparedMailingAddress { get; set; }

    public DateTime? PreferredTimeToContact { get; set; }

    public DateTime DateCreated { get; set; }

    public int CreatedById { get; set; }

    public DateTime? DateModified { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateDeleted { get; set; }

    public int? DeletedById { get; set; }

    public string PropertyDeveloperName { get; set; }

    public string PropertyLocation { get; set; }

    public string PropertyUnitLevelName { get; set; }
}