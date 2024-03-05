using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.UserDto;
using Template.Domain.Dto.EmailSettingsDto;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using Template.Application.Services;

namespace Template.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private EmailSettingsModel _emailSettings;
        public EmailService(IOptions<EmailSettingsModel> emailSettings) { 
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(List<string> sendToEmails, string subject, MimeEntity body)
        {
            try
            {
                MimeMessage emailMessage = new();
                List<MailboxAddress> emailList = sendToEmails.Select(email => MailboxAddress.Parse(email)).ToList();

                emailMessage.Sender = MailboxAddress.Parse(_emailSettings.Email);
                emailMessage.To.AddRange(emailList);
                emailMessage.Subject = subject;
                emailMessage.Body = body;

                using MailKit.Net.Smtp.SmtpClient smtp = new();
                smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.Auto);
                smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
                await smtp.SendAsync(emailMessage);
                smtp.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SendUserInfo(UserModel model)
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
                                                        Your account has been created on the eiDOC Partnership with JuanTahanan Project. Below are your system generated credentials, <br><strong>Please change
                                                            the password immediately after login</strong>.</p>
                                                    <span
                                                        style=""display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;""></span>
                                                    <p
                                                        style=""color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;"">
                                                        <strong
                                                            style=""display: block;font-size: 13px; margin: 0 0 4px; color:rgba(0,0,0,.64); font-weight:normal;"">Username</strong>{1}
                                                        <strong
                                                            style=""display: block; font-size: 13px; margin: 24px 0 4px 0; font-weight:normal; color:rgba(0,0,0,.64);"">Password</strong>{2}
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
            body = body.Replace("{0}", model.Name).Replace("{1}", model.UserName).Replace("{2}", "Pass123$");

            var emailBody = new TextPart("html")
            {
                Text = body
            };
            var emails = new List<string> { model.Email };
            var subject = $"Welcome {model.Name} to project JuanTahanan!";
            await SendEmailAsync(emails, subject, emailBody);
        }


    }
}
