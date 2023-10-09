using MHealth.Models.DTO;

namespace MHealth.Repositories.Abstract
{
    public interface IEmailRepository
    {
        Task SendEmail(string fromEmail, string toEmail, string subject, string message, string attachmentPath = null);
    }
}
