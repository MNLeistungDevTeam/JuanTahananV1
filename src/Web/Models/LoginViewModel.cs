﻿using DMS.Domain.Dto.CompanyDto;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DMS.Web.Models;

public class LoginViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "User name is Required!")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Password is Required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DisplayName("Remember Me")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; } = string.Empty;

    //[Required]
    [DisplayName("Company")]
    public int? CompanyId { get; set; }

    public List<CompanyModel>? Company { get; set; }

    public string? RecoveryEmail { get; set; }
}