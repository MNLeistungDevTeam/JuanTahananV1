using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.RoleDto
{
    public class RoleAccessModel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int ModuleId { get; set; }

        public bool CanCreate { get; set; }

        public bool CanModify { get; set; }

        public bool CanDelete { get; set; }

        public bool CanRead { get; set; }

        public bool FullAccess
        {
            get
            {
                if (CanRead && CanDelete && CanModify && CanCreate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int CreatedById { get; set; }

        public DateTime DateCreated { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
