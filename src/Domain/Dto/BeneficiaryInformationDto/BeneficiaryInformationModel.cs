using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.BeneficiaryInformationDto
{
    public class BeneficiaryInformationModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [Display(Name = "Pagibig Number", Prompt = "Pagibig Number")]
        public string? PagibigNumber { get; set; }

        public int? CompanyId { get; set; }

        [Required]
        [Display(Name = "Last Name", Prompt = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name", Prompt = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name", Prompt = "Middle Name")]
        public string MiddleName { get; set; }

        public string Name
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }

        [Required]
        [Display(Name = "Birth Date", Prompt = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        public int? Age { get; set; }

        [Required]
        [Display(Name = "Mobile Number", Prompt = "Mobile Number")]
        public string? MobileNumber { get; set; }

        [Required]
        [Display(Name = "Gender", Prompt = "Gender")]
        public string? Sex { get; set; }

        public bool IsPermanentAddressAbroad { get; set; }

        public bool IsPresentAddressAbroad { get; set; }

        [Required]
        [Display(Name = "Present Unit Name", Prompt = "Present Unit Name")]
        public string PresentUnitName { get; set; }

        [Display(Name = "Present Building Name", Prompt = "Present Building Name")]
        [Required]
        public string PresentBuildingName { get; set; }

        [Required]
        [Display(Name = "Present Lot Name", Prompt = "Present Lot Name")]
        public string PresentLotName { get; set; }

        [Required]
        [Display(Name = "Present Street Name", Prompt = "Present Street Name")]
        public string PresentStreetName { get; set; }

        [Required]
        [Display(Name = "Present Subdivision Name", Prompt = "Present Subdivision Name")]
        public string PresentSubdivisionName { get; set; }

        [Required]
        [Display(Name = "Present Barangay Name", Prompt = "Present Barangay Name")]
        public string PresentBaranggayName { get; set; }

        [Required]
        [Display(Name = "Present Municipality Name", Prompt = "Present Municipality Name")]
        public string PresentMunicipalityName { get; set; }

        [Required]
        [Display(Name = "Present Province Name", Prompt = "Present Province Name")]
        public string PresentProvinceName { get; set; }

        [Required]
        [Display(Name = "Present Zip Code", Prompt = "Present Zip Code")]
        public string PresentZipCode { get; set; }

        [Display(Name = "Permanent Unit Name", Prompt = "Permanent Unit Name")]
        public string? PermanentUnitName { get; set; }

        [Display(Name = "Permanent Building Name", Prompt = "Permanent Building Name")]
        public string? PermanentBuildingName { get; set; }

        [Display(Name = "Permanent Lot Name", Prompt = "Permanent Lot Name")]
        public string? PermanentLotName { get; set; }

        [Display(Name = "Permanent Street Name", Prompt = "Permanent Street Name")]
        public string? PermanentStreetName { get; set; }

        [Display(Name = "Permanent Subdivision Name", Prompt = "Permanent Subdivision Name")]
        public string? PermanentSubdivisionName { get; set; }

        [Display(Name = "Permanent Baranggay Name", Prompt = "Permanent Baranggay Name")]
        public string? PermanentBaranggayName { get; set; }

        [Display(Name = "Permanent Municipality Name", Prompt = "Permanent Municipality Name")]
        public string? PermanentMunicipalityName { get; set; }

        [Display(Name = "Permanent Province Name", Prompt = "Permanent Province Name")]
        public string? PermanentProvinceName { get; set; }

        [Display(Name = "Permanent Zip Code", Prompt = "Permanent Zip Code")]
        public string? PermanentZipCode { get; set; }

        [Display(Name = "Property Developer Name", Prompt = "Property Developer Name")]
        public string? PropertyDeveloperName { get; set; }

        [Display(Name = "Property Location", Prompt = "Property Location")]
        public string? PropertyLocation { get; set; }

        [Display(Name = "Property Unit Level Name", Prompt = "Property Unit Level Name")]
        public string? PropertyUnitLevelName { get; set; }

        public int? CreatedById { get; set; }

        public DateTime DateCreated { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateModified { get; set; }

        [Display(Name = "Email", Prompt = "Email")]
        public string? Email { get; set; }

        public string? ProfilePicture { get; set; } = string.Empty;
    }
}