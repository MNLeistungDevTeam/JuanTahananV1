﻿namespace DMS.Domain.Dto.BuyerConfirmationDto;

public class BuyerConfirmationExcelModel
{
    public string? FullName { get; set; }
    public string? isPagibigMember { get; set; }
    public string? PagibigNumber { get; set; }
    public string? BirthDate { get; set; }
    public decimal? MonthlySalary { get; set; }
    public string? PresentHomeAddress { get; set; }
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public string? EmployerName { get; set; }
    public string? EmployerContactPerson { get; set; }
    public string? EmployerContactNumber { get; set; }
    public string? EmployerEmailAddress { get; set; }


    public string? PropertyProjectName { get; set; }
    public string? PropertyDeveloperName { get; set; }
    public string? PropertyLocationName { get; set; }

    public int PropertyDeveloperId { get; set; }
    public int PropertyProjectId { get; set; }
    public int ProjectUnitId { get; set; }

    public int PropertyLocationId { get; set; }
    public DateTime? LastUpdate { get; set; }
}