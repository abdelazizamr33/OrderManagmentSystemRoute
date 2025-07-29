using BLL.Interfaces.Services;
using DAL.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using OrderManagmentSystem;


namespace BLL.Services
{
    public class MailService : IMailService
    {
        private readonly IOptions<MailSettings> _options;

        public MailService(IOptions<MailSettings> options)
        {
            _options = options;
        }

        public void SendEmail(Email email)
        {
            var mail = new MimeMessage();
            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder
            {
                TextBody = email.Body
            };
            mail.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email, _options.Value.Password);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }
    }

}
