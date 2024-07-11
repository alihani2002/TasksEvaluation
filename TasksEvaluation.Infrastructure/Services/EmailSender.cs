using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TasksEvaluation.Infrastructure.Helpers;


namespace TasksEvaluation.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings mailSettings;
        private readonly IWebHostEnvironment webHostEnvironment;
        public EmailSender(IOptions<MailSettings> _mailSettings, IWebHostEnvironment _webHostEnvironment)
        {
            mailSettings = _mailSettings.Value;
            webHostEnvironment = _webHostEnvironment;
        }
        public async Task SendEmailAsync(string email, string subject, string content)
        {
            if (string.IsNullOrEmpty(email))
                return;

            var message = new MailMessage();
            message.From = new MailAddress(mailSettings.Email!, mailSettings.DisplayName);
            message.Subject = subject;
            message.To.Add(email);
            message.Body = $"<html><body>{content}</body></html>";
            message.IsBodyHtml = true;


            var smtpClient = new SmtpClient(mailSettings.Host)
            {
                Port = mailSettings.Port,
                Credentials = new NetworkCredential(mailSettings.Email, mailSettings.Password),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(message);
            smtpClient.Dispose();
        }
    }
}
