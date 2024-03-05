using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.UserDto;

namespace Template.Application.Services
{
    public interface IEmailService
    {
        Task SendUserInfo(UserModel model);
        Task SendEmailAsync(List<string> sendToEmails, string subject, MimeEntity body);
    }
}
