using Notes.EmailService.Models;

namespace Notes.EmailService;

public interface IEmailSender
{
    Task SendEmailAsync(EmailModel model);
    Task SendEmailAsync(string email, string subject, string message);
}
