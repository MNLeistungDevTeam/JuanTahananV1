using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.UserDto;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Services
{
    public interface IEmailService
    {
        Task SendUserCredential(UserModel model);
        Task SendEmailAsync(List<string> sendToEmails, string subject, MimeEntity body);
        Task SendUserConfirmationMessage(UserModel model);
        Task SendApplicationStatus(ApplicantsPersonalInformationModel model, string receiverEmail);
    }
}
