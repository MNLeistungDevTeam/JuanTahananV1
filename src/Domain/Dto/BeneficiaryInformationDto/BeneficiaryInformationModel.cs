using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.BeneficiaryInformationDto
{
    public class BeneficiaryInformationModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Code { get; set; }

        [Required]
        [Display(Name = "Pag-IBIG MID Number/RTN", Prompt = "XXXX-XXXX-XXXX")]
        public string? PagibigNumber { get; set; }

        public int? CompanyId { get; set; }

        [Required]
        [Display(Name = "Last Name", Prompt = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "First Name", Prompt = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name", Prompt = "Middle Name")]
        public string? MiddleName { get; set; }

        public string? Name
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
        [Display(Name = "Cell Phone", Prompt = "Mobile Number", Description = "Cell Phone / Mobile Number")]
        public string? MobileNumber { get; set; }

        [Required]
        [Display(Name = "Gender", Prompt = "Gender")]
        public string? Sex { get; set; }

        [DisplayName("Is Permanent Address on Abroad")]
        public bool IsPermanentAddressAbroad { get; set; }

        [DisplayName("Is Present Address on Abroad")]
        public bool IsPresentAddressAbroad { get; set; }

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

        [Display(Name = "Unit/Room No., Floor", Prompt = "Unit Name", Description = "Unit/Room No., Floor")]
        public string? PermanentUnitName { get; set; }

        [Display(Name = "Building Name", Prompt = "Building Name", Description = "Building Name")]
        public string? PermanentBuildingName { get; set; }

        [Display(Name = "Lot No., Blk No., Phase No., House No.", Prompt = "Lot Name", Description = "Lot No., Blk No., Phase No., House No.")]
        public string? PermanentLotName { get; set; }

        [Display(Name = "Street Name", Prompt = "Street Name")]
        public string? PermanentStreetName { get; set; }

        [Display(Name = "Subdivision", Prompt = "Subdivision Name", Description = "Subdivision")]
        public string? PermanentSubdivisionName { get; set; }

        [Display(Name = "Barangay", Prompt = "Barangay Name", Description = "Barangay")]
        public string? PermanentBaranggayName { get; set; }

        [Display(Name = "Municipality/City", Prompt = "Municipality/City Name", Description = "Municipality/City")]
        public string? PermanentMunicipalityName { get; set; }

        [Display(Name = "Province and State Country (if abroad)", Prompt = "Province Name", Description = "Province and State Country (if abroad)")]
        public string? PermanentProvinceName { get; set; }

        [Display(Name = "ZIP Code", Prompt = "Zip Code")]
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

        [Display(Name = "Email Address", Prompt = "Email")]
        public string? Email { get; set; }

        public string? ProfilePicture { get; set; } = string.Empty;

        public int InitialAge
        {
            set
            {
                DateTime now = DateTime.Now;
                int age = now.Year - BirthDate.Value.Year;
                if (now < BirthDate.Value.AddYears(age))
                {
                    age--;
                }
                Age = age;
            }
        }

        public bool IsBcfCreated { get; set; }

        [Required]
        [Display(Name = "Developer")]
        public int PropertyDeveloperId { get; set; }

        [Display(Name = "Location")]
        public int PropertyLocationId { get; set; }

        [Display(Name = "Project")]
        public int PropertyProjectId { get; set; }

        [Display(Name = "House Unit")]
        public int PropertyUnitId { get; set; }

        public string? PropertyUnitDescription { get; set; }
        public string? DeveloperName { get; set; }
        public string? PropertyLocationName { get; set; }

        public string? PropertyProjectName { get; set; }
    }
}