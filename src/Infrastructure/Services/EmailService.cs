using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using DMS.Application.Services;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Dto.EmailSettingsDto;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.ModuleDto;
using DMS.Domain.Dto.TemporaryLinkDto;
using DMS.Domain.Entities;
using DMS.Application.Interfaces.Setup.EmailSetupRepo;

namespace DMS.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private EmailSettingsModel _emailSettings;
        private IEmailSetupRepository _emailSetupRepo;


        public EmailService(IOptions<EmailSettingsModel> emailSettings,IEmailSetupRepository emailSetupRepo)
        {
            _emailSettings = emailSettings.Value;
            _emailSetupRepo = emailSetupRepo;
        }

        public async Task SendEmailAsync(List<string> sendToEmails, string subject, MimeEntity body, int companyId)
        {
            EmailLog emailLog = new();

            try
            {
                //MimeMessage emailMessage = new();
                //List<MailboxAddress> emailList = sendToEmails.Select(email => MailboxAddress.Parse(email)).ToList();

                //emailMessage.Sender = MailboxAddress.Parse(_emailSettings.Email);
                //emailMessage.To.AddRange(emailList);
                //emailMessage.Subject = subject;
                //emailMessage.Body = body;

                //using MailKit.Net.Smtp.SmtpClient smtp = new();
                //smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.Auto);
                //smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
                //await smtp.SendAsync(emailMessage);
                //smtp.Disconnect(true);

                var setup = await _emailSetupRepo.GetByCompany(companyId);
                MimeMessage emailMessage = new();
                List<MailboxAddress> emailList = sendToEmails.Select(email => MailboxAddress.Parse(email)).ToList();
                var emailAddress = new List<MailboxAddress>();

                emailAddress.Add(MailboxAddress.Parse(setup.Email.Trim()));
                emailMessage.From.AddRange(emailAddress);
                emailMessage.Sender = MailboxAddress.Parse(setup.Email.Trim());
                emailMessage.To.AddRange(emailList);
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.Date = DateTime.Now;
                //emailMessage.ReplyTo.Add(new MailboxAddress("Jericho ", "jericho.mosqueda@mnleistung.de"));

                using MailKit.Net.Smtp.SmtpClient smtp = new();
                smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true; // Bypass certificate validation (for testing only)
                var forGmailTLS = SecureSocketOptions.StartTls;
                var forSSL = SecureSocketOptions.SslOnConnect;
                var forAuto = SecureSocketOptions.Auto;
                smtp.Connect(setup.Host, setup.Port, forAuto);
                smtp.Authenticate(setup.Email.Trim(), setup.Password);
                await smtp.SendAsync(emailMessage);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                emailLog.Description = ex.Message;
                emailLog.Status = "Failed";
            }
        }

        //public async Task SendEmailAsync(string sendToEmail, string subject, MimeEntity body, int companyId, EmailLog logs)
        //{
        //    try
        //    {
        //        MimeMessage emailMessage = new();

        //        List<string> sendToEmails = new();
        //        sendToEmails.Add(sendToEmail);

        //        await SendEmailAsync(sendToEmails, subject, body, companyId, logs);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task SendApplicationStatus(ApplicantsPersonalInformationModel model, string receiverEmail)
        {
            string body = @"
            <!doctype html>
            <html lang=""en-US"">
            <head>
                <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"" />
                <title>New Account Email Template</title>
                <meta name=""description"" content=""Welcome to project JuanTahanan!"">
                <style type=""text/css"">
                    a:hover {text-decoration: underline !important;}
                </style>
            </head>
            <body marginheight=""0"" topmargin=""0"" marginwidth=""0"" style=""margin: 0px; background-color: #f2f3f8;"" leftmargin=""0"">
                <!-- 100% body table -->
                <table cellspacing=""0"" border=""0"" cellpadding=""0"" width=""100%"" bgcolor=""#f2f3f8""
                    style=""@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;"">
                    <tr>
                        <td>
                            <table style=""background-color: #f2f3f8; max-width:670px; margin:0 auto;"" width=""100%"" border=""0""
                                align=""center"" cellpadding=""0"" cellspacing=""0"">
                                <tr>
                                    <td style=""height:80px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""text-align:center;"">
                                        <a href=""https://juantahanan-dms.mnleistung.ph"" title=""logo"" target=""_blank"">
                                        <img width=""400"" src=""https://i.ibb.co/hy7tVzG/Juan-Tahanan-Logo.png"" title=""logo"" alt=""logo"">
                                      </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:20px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width=""95%"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0""
                                            style=""max-width:670px; background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);"">
                                            <tr>
                                                <td style=""height:40px;"">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style=""padding:0 35px;"">
                                                    <h1 style=""color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;"">
                                                    </h1>
                                                    <p style=""font-size:15px; color:#455056; margin:8px 0 0; line-height:24px;"">
                                                        Your application has been {applicationStatus} by {approverName}  ({approverRole}) on the eiDOC Partnership with JuanTahanan Project. Below are your current application details, <br></p>
                                                    <span
                                                        style=""display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;""></span>
                                                     <h1 style=""color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;"">
                                                        Application No: {applicationNo}
                                                    </h1>
                                                    <p style=""color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                                        <strong
                                                            style=""display: block;font-size: 13px; margin: 0 0 4px; color:rgba(0,0,0,.64); font-weight:normal;"">Current Stage:</strong> {stage}
                                                        <strong
                                                    </p>
                                                    <p style=""color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                                        <strong
                                                            style=""display: block;font-size: 13px; margin: 0 0 4px; color:rgba(0,0,0,.64); font-weight:normal;"">Remarks:</strong> {remarks}
                                                        <strong
                                                    </p>

                                                    <a href=""https://juantahanan-dms.mnleistung.ph/Applicants/Details/{applicationNo}""
                                                        style=""background:#FEC10E;text-decoration:none !important; display:inline-block; font-weight:500; margin-top:24px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;"">View
                                                        Application</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style=""height:40px;"">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:20px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""text-align:center;"">
                                        <p style=""font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;"">&copy; <strong>juantahanan-dms.mnleistung.ph</strong> </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:80px;"">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!--/100% body table-->
            </body>

            </html>
    ";
            body = body.Replace("{applicationNo}", model.Code).Replace("{stage}", model.Stage).Replace("{remarks}", model.Remarks).Replace("{applicationStatus}", model.ApplicationStatus)
                .Replace("{approverName}", model.ApproverFirstName).Replace("{approverRole}", model.ApproverRole);

            var emailBody = new TextPart("html")
            {
                Text = body
            };
            var emails = new List<string> { receiverEmail };
            var subject = $"Good Day {model.ApplicantFirstName} your application is already  processed";
            await SendEmailAsync(emails, subject, emailBody, model.CompanyId.Value);
        }

        public async Task SendUserCredential(UserModel model)
        {
            string body = @"
            <!doctype html>
            <html lang=""en-US"">
            <head>
                <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"" />
                <title>New Account Email Template</title>
                <meta name=""description"" content=""Welcome to project JuanTahanan, {0}!"">
                <style type=""text/css"">
                    a:hover {text-decoration: underline !important;}
                </style>
            </head>
            <body marginheight=""0"" topmargin=""0"" marginwidth=""0"" style=""margin: 0px; background-color: #f2f3f8;"" leftmargin=""0"">
                <!-- 100% body table -->
                <table cellspacing=""0"" border=""0"" cellpadding=""0"" width=""100%"" bgcolor=""#f2f3f8""
                    style=""@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;"">
                    <tr>
                        <td>
                            <table style=""background-color: #f2f3f8; max-width:670px; margin:0 auto;"" width=""100%"" border=""0""
                                align=""center"" cellpadding=""0"" cellspacing=""0"">
                                <tr>
                                    <td style=""height:80px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""text-align:center;"">
                                        <a href=""https://juantahanan-dms.mnleistung.ph"" title=""logo"" target=""_blank"">
                                        <img width=""400"" src=""https://i.ibb.co/hy7tVzG/Juan-Tahanan-Logo.png"" title=""logo"" alt=""logo"">
                                      </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:20px;"">&nbsp;</td>
                                </tr>
                               <tr>
                        <td>
                            <table width=""95%"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0""
                                   style=""max-width:670px; background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);"">
                                <tr>
                                    <td style=""height:40px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""padding:0 35px;"">
                                        <h1 style=""color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;"">Get started</h1>
                                        <p style=""font-size:15px; color:#455056; margin:8px 0 0; line-height:24px;"">Your account has been {actiontype} on the eiDOC Partnership with JuanTahanan Project. Below are your system generated credentials,<br><strong>Please change the password immediately after login</strong>.</p>
                                        <span style=""display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;""></span>
                                        <p style=""color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                            <span style=""font-size: 13px; color:rgba(0,0,0,.64); font-weight:normal;"">Username: </span>
                                            <strong style=""display: block;font-size: 16px; margin: 0 0 4px; color:#1e1e2d; font-weight:bold;"">{1}</strong>
                                        </p>
                                        <p style=""margin-bottom: 1em;""></p> <!-- Add 1 line spacing -->
                                        <p style=""color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                            <span style=""font-size: 13px; color:rgba(0,0,0,.64); font-weight:normal;"">Password: </span>
                                            <strong style=""display: block;font-size: 16px; margin: 0 0 4px; color:#1e1e2d; font-weight:bold;"">{2}</strong>
                                        </p>

                                        <a href=""https://testjuantahanan.mnleistung.ph/"" style=""background:#FEC10E;text-decoration:none !important; display:inline-block; font-weight:500; margin-top:20px; margin-bottom: 30px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;"">Login to your Account</a>

                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style=""height:20px;"">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style=""text-align: center;"">
                            <p style=""font-size: 14px; color: rgba(69, 80, 86, 0.7411764705882353); line-height: 18px; margin: 0 0 0;"">
                            &copy; <strong>testjuantahanan.mnleistung.ph</strong>
                            </p>
                        </td>
                    </tr>

                            </table>
                        </td>
                    </tr>
                </table>
                <!--/100% body table-->
            </body>

            </html>
    ";
            body = body.Replace("{0}", model.Name).Replace("{1}", model.UserName).Replace("{2}", model.Password).Replace("{actiontype}", model.Action);

            var emailBody = new TextPart("html")
            {
                Text = body
            };
            var emails = new List<string> { model.Email };
            var subject = $"Welcome {model.Name} to project JuanTahanan!";
            await SendEmailAsync(emails, subject, emailBody,model.CompanyId.Value);
        }

        public async Task SendUserCredential2(UserModel? model, string? rootFolder)
        {
            //HTML Body
            string body = string.Empty;
            string filepath = Path.Combine(rootFolder, "EmailTemplate", "UserCredentialTemplate.html");
            using (StreamReader str = new(filepath))
            {
                body = str.ReadToEnd();
            }

            //string filePath = Path.Combine("EmailTemplate", "UserCredentialTemplate.html");
            //string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //string fullPath = Path.Combine(rootDirectory, filePath);

            body = body.Replace("{name}", model.Name).Replace("{userName}", model.UserName).Replace("{password}", model.Password).Replace("{actiontype}", model.Action);

            var emails = new List<string> { model.Email };
            var subject = $"Welcome {model.Name} to project JuanTahanan!";

            var builder = new BodyBuilder();
            builder.HtmlBody = body;

            //Sending of Email
            MimeEntity generatedbody = builder.ToMessageBody();
            await SendEmailAsync(emails, subject, generatedbody,model.CompanyId.Value);
        }

        public async Task SendUserConfirmationMessage(UserModel model)
        {
            string body = @"
            <!doctype html>
            <html lang=""en-US"">
            <head>
                <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"" />
                <title>Password Reset Confirmation</title>
                <meta name=""description"" content=""Welcome to project JuanTahanan, {0}!"">
                <style type=""text/css"">
                    a:hover {text-decoration: underline !important;}
                </style>
            </head>
            <body marginheight=""0"" topmargin=""0"" marginwidth=""0"" style=""margin: 0px; background-color: #f2f3f8;"" leftmargin=""0"">
                <!-- 100% body table -->
                <table cellspacing=""0"" border=""0"" cellpadding=""0"" width=""100%"" bgcolor=""#f2f3f8""
                    style=""@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;"">
                    <tr>
                        <td>
                            <table style=""background-color: #f2f3f8; max-width:670px; margin:0 auto;"" width=""100%"" border=""0""
                                align=""center"" cellpadding=""0"" cellspacing=""0"">
                                <tr>
                                    <td style=""height:80px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""text-align:center;"">
                                        <a href=""https://juantahanan-dms.mnleistung.ph"" title=""logo"" target=""_blank"">
                                        <img width=""400"" src=""https://i.ibb.co/hy7tVzG/Juan-Tahanan-Logo.png"" title=""logo"" alt=""logo"">
                                      </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:20px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width=""95%"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0""
                                            style=""max-width:670px; background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);"">
                                            <tr>
                                                <td style=""height:40px;"">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style=""padding:0 35px;"">
                                                    <h1 style=""color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;"">Get started
                                                    </h1>
                                                    <p style=""font-size:15px; color:#455056; margin:8px 0 0; line-height:24px;"">
                                                        Your account requested to {actiontype} on the eiDOC Partnership with JuanTahanan Project.<br></p>
                                                        <p> To reset your password visit the following address:</p>
                                                    <span
                                                        style=""display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;""></span>
                                                    <p style=""color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                                        <strong
                                                            style=""display: block;font-size: 13px; margin: 0 0 4px; color:rgba(0,0,0,.64); font-weight:normal;"">Email:</strong> {1}
                                                        <strong
                                                    </p>

                                                 <a href=""https://juantahanan-dms.mnleistung.ph""
                                                        style=""background:#FEC10E;text-decoration:none !important; display:inline-block; font-weight:500; margin-top:24px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;""> Confirm
                                                        to your Account</a>
                                                    <p  style=""color:#455056;margin-top: 2px;font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                                        <strong
                                                            style=""display: block;font-size: 13px; margin: 0 0 4px; color:rgba(0,0,0,.64); font-weight:normal;"">If this was a mistake, just ignore this email and nothing will happen</strong>
                                                        <strong
                                                    </p>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td style=""height:40px;"">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:20px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""text-align:center;"">
                                        <p style=""font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;"">&copy; <strong>juantahanan-dms.mnleistung.ph</strong> </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:80px;"">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!--/100% body table-->
            </body>

            </html>
    ";
            body = body.Replace("{0}", model.Name).Replace("{1}", model.Email).Replace("{actiontype}", model.Action);

            var emailBody = new TextPart("html")
            {
                Text = body
            };
            var emails = new List<string> { model.Email };
            var subject = $"Welcome {model.Name} to project JuanTahanan!";
            await SendEmailAsync(emails, subject, emailBody,model.CompanyId.Value);
        }

        public async Task SendApplicationStatusToBeneficiary(UserModel model)
        {
            string body = @"
            <!doctype html>
            <html lang=""en-US"">
            <head>
                <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"" />
                <title>New Account Email Template</title>
                <meta name=""description"" content=""Welcome to project JuanTahanan, {0}!"">
                <style type=""text/css"">
                    a:hover {text-decoration: underline !important;}
                </style>
            </head>
            <body marginheight=""0"" topmargin=""0"" marginwidth=""0"" style=""margin: 0px; background-color: #f2f3f8;"" leftmargin=""0"">
                <!-- 100% body table -->
                <table cellspacing=""0"" border=""0"" cellpadding=""0"" width=""100%"" bgcolor=""#f2f3f8""
                    style=""@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;"">
                    <tr>
                        <td>
                            <table style=""background-color: #f2f3f8; max-width:670px; margin:0 auto;"" width=""100%"" border=""0""
                                align=""center"" cellpadding=""0"" cellspacing=""0"">
                                <tr>
                                    <td style=""height:80px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""text-align:center;"">
                                        <a href=""https://juantahanan-dms.mnleistung.ph"" title=""logo"" target=""_blank"">
                                        <img width=""400"" src=""https://i.ibb.co/hy7tVzG/Juan-Tahanan-Logo.png"" title=""logo"" alt=""logo"">
                                      </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:20px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width=""95%"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0""
                                            style=""max-width:670px; background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);"">
                                            <tr>
                                                <td style=""height:40px;"">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style=""padding:0 35px;"">
                                                    <h1 style=""color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;"">Get started
                                                    </h1>
                                                    <p style=""font-size:15px; color:#455056; margin:8px 0 0; line-height:24px;"">
                                                        Your account has been {actiontype} on the eiDOC Partnership with JuanTahanan Project. Below are your system generated credentials, <br><strong>Please change
                                                            the password immediately after login</strong>.</p>
                                                    <span
                                                        style=""display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;""></span>
                                                    <p style=""color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                                        <strong
                                                            style=""display: block;font-size: 13px; margin: 0 0 4px; color:rgba(0,0,0,.64); font-weight:normal;"">Username:</strong> {1}
                                                        <strong
                                                    </p>
                                                    <p style=""color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                                        <strong
                                                            style=""display: block;font-size: 13px; margin: 0 0 4px; color:rgba(0,0,0,.64); font-weight:normal;"">Password:</strong> {2}
                                                        <strong
                                                    </p>

                                                    <a href=""https://juantahanan-dms.mnleistung.ph""
                                                        style=""background:#FEC10E;text-decoration:none !important; display:inline-block; font-weight:500; margin-top:24px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;"">Login
                                                        to your Account</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style=""height:40px;"">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:20px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""text-align:center;"">
                                        <p style=""font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;"">&copy; <strong>juantahanan-dms.mnleistung.ph</strong> </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""height:80px;"">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!--/100% body table-->
            </body>

            </html>
    ";
            body = body.Replace("{0}", model.Name).Replace("{1}", model.UserName).Replace("{2}", model.Password).Replace("{actiontype}", model.Action);

            var emailBody = new TextPart("html")
            {
                Text = body
            };
            var emails = new List<string> { model.Email };
            var subject = $"Welcome {model.Name} to project JuanTahanan!";
            await SendEmailAsync(emails, subject, emailBody,model.CompanyId.Value);
        }
    }
}