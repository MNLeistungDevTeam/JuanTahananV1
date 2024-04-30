using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.EmailSetupDto
{
    public class EmailSetupModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Host is required.")]
        public string Host { get; set; }

        [Required(ErrorMessage = "Display Name is required.")]
        [DisplayName("Display Name")]
        public string? DisplayName { get; set; }

        [Required(ErrorMessage = "Port is required.")]
        public int Port { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [DisplayName("Company")]
        public int CompanyId { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedById { get; set; }
        public DateTime? DateModified { get; set; }
        public int? ModifiedById { get; set; }

        #region Display Properties

        public string? CompanyName { get; set; }
        public bool IsDefault { get; set; }

        #endregion Display Properties
    }
}
