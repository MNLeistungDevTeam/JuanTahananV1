using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Dto.ReferenceDto;
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
 
        Task SendEmailAsync(List<string> sendToEmails, string subject, MimeEntity body, int companyId, ReferenceModel refModel);

        Task SendUserCredential2(UserModel? model, string? rootFolder);

        Task SendUserCredential(UserModel model);

        Task SendUserConfirmationMessage(UserModel model);

        Task SendApplicationStatus(ApplicantsPersonalInformationModel model, string receiverEmail);
        Task SendApplicationStatusToBeneficiary(ApplicantsPersonalInformationModel model, string receiverEmail, string? rootFolder);
        Task SendBuyerConfirmationStatusToBeneficiary(BuyerConfirmationModel model, string receiverEmail, string? rootFolder);
        Task SendUserCredentialResetConfirmation(UserModel model, string? rootFolder, string? baseUrl);
    }
}