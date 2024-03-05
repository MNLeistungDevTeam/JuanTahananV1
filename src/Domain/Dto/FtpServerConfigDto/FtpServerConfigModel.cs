using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Dto.FtpServerConfigDto
{
    public class FtpServerConfigModel
    {
        public string? FtpUser { get; set; }
        public string? FtpPass { get; set; }
        public string? FtpHost { get; set; }
    }
}
