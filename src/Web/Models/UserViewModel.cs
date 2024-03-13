using DMS.Domain.Dto.OtherDto;
using DMS.Domain.Dto.UserDto;
using System.Collections.Generic;

namespace DMS.Web.Models
{
    public class UserViewModel
    {
        public UserModel? User { get; set; }
        public List<UserApproverModel?> UserApprover { get; set; }

        public ChangePasswordModel? ChangePassword { get; set; }
    }
}