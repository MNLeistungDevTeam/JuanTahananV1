using System;
using System.ComponentModel.DataAnnotations;

namespace DMS.Web.Models;

public class ReportFilterModel
{
    [Display(Name = "Date from")]
    public DateTime DateFrom { get; set; }

    [Display(Name = "Date to")]
    public DateTime DateTo { get; set; }

    [Display(Name = "Created by")]
    public int PreparedBy { get; set; }
}