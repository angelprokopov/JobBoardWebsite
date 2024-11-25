using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace JobBoard.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly AuthMessageSenderOptions options;

        public EmailSender (IOptions<AuthMessageSenderOptions> config)
        {
            options = config.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(options.SendGridApiKey);
            var from = new EmailAddress("", "");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            var response = await client.SendEmailAsync(message);
        }
    }
}
