﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DMS.Domain.Entities;

public partial class Spouse
{
    public int Id { get; set; }

    public int ApplicantsPersonalInformationId { get; set; }

    public bool? IsSpouseAddressAbroad { get; set; }

    public string SpouseEmploymentUnitName { get; set; }

    public string SpouseEmploymentBuildingName { get; set; }

    public string SpouseEmploymentLotName { get; set; }

    public string SpouseEmploymentStreetName { get; set; }

    public string SpouseEmploymentSubdivisionName { get; set; }

    public string SpouseEmploymentBaranggayName { get; set; }

    public string SpouseEmploymentMunicipalityName { get; set; }

    public string SpouseEmploymentProvinceName { get; set; }

    public string SpouseEmploymentZipCode { get; set; }

    public string PreparedMailingAddress { get; set; }

    public DateTime? PreferredTimeToContact { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string Suffix { get; set; }

    public string PagibigMidNumber { get; set; }

    public string TinNumber { get; set; }

    public int? IndustryId { get; set; }

    public int? BusinessNumber { get; set; }

    public string Citizenship { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime DateCreated { get; set; }

    public int CreatedById { get; set; }

    public DateTime? DateModified { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateDeleted { get; set; }

    public int? DeletedById { get; set; }

    public string BusinessName { get; set; }

    public string OccupationStatus { get; set; }

    public int? YearsInEmployment { get; set; }

    public string EmploymentPosition { get; set; }

    public string BusinessTelNo { get; set; }
}