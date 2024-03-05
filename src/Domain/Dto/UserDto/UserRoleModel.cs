using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Dto.UserDto
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Users", Prompt = "Select users")]
        [Required(ErrorMessage ="this field is required")]
        public int[]? UsersId { get; set; }  
        public int RoleId { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime Status { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public string? Email { get; set; }
        public string? ProfilePicture { get; set; }
        public string Name
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }
        public string StatusOnline
        {
            get
            {
                // Calculate the difference between the current date and the JoinedDate
                TimeSpan timeSinceJoined = DateTime.Now - Status;

                if (timeSinceJoined.TotalDays < 1)
                {
                    // User is online
                    return "Online";
                }
                else if (timeSinceJoined.TotalDays < 365)
                {
                    // User has been offline less than a year
                    int days = (int)timeSinceJoined.TotalDays;
                    return $"Offline {days} {(days == 1 ? "day" : "days")} ago";
                }
                else
                {
                    // User has been offline for a year or more
                    int years = (int)(timeSinceJoined.TotalDays / 365);
                    return $"Offline {years} {(years == 1 ? "year" : "years")} ago";
                }
            }
        }
    }
}
