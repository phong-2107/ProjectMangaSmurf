using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProjectMangaSmurf.Repository
{
    public class EFEmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;
        public EFEmailRepository( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(ContactMail model)
        {
            var senderEmail = _configuration["EmailSettings:Sender"];
            var senderName = _configuration["EmailSettings:SenderName"];

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = $"{model.Subject} (from {model.Name} - {model.Email})",
                Body = $"From: {model.Name} <{model.Email}>\n\n{model.Message}",
                IsBodyHtml = true
            };
            mailMessage.To.Add(senderEmail);  // Gửi đến địa chỉ đã khai báo trong JSON

            using (var smtpClient = new SmtpClient(_configuration["EmailSettings:MailServer"], int.Parse(_configuration["EmailSettings:MailPort"])))
            {
                smtpClient.Credentials = new NetworkCredential(senderEmail, _configuration["EmailSettings:Password"]);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
