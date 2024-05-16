using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailTemplateAsync(string email, string subject, string message, string template);

        public Task<string> ReadTemplateAsync(string templateName);
    }
}