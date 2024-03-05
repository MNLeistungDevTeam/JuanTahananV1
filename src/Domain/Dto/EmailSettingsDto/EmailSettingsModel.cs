﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.EmailSettingsDto
{
    public class EmailSettingsModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public string? Displayname { get; set; }
        public int Port { get; set; }
    }
}
