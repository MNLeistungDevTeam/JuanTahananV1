using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Dto.UserDto
{
    public class UserDocumentModel
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public int DocumentTypeId { get; set; }

        public int ApplicantsPersonalInformationId { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }
    }
}
