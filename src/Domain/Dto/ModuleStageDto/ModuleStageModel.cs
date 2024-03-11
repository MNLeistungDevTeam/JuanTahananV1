using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ModuleStageDto
{

    public class ModuleStageModel
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public int Level { get; set; }
        public string? ApproveDesc { get; set; }
        public string? RejectDesc { get; set; }
        public int ReturnStage { get; set; }
        public int RequiredCount { get; set; }
        public int ApproverId { get; set; }
        public bool IsDisabled { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedById { get; set; }
        public DateTime DateModified { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? UserName { get; set; }
        public string? ApproverFullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
