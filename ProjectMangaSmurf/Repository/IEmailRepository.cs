using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(ContactMail model);
    }
}
