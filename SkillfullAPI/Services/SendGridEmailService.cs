using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using SkillfullAPI.Services.Interfaces;

namespace SkillfullAPI.Services
{
    public class SendGridEmailService : ISendGridEmailService
    {
        private readonly ILogger<SendGridEmailService> _logger;
        private readonly IConfiguration _configuration;
        public SendGridEmailService(IConfiguration configuration, ILogger<SendGridEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            string apiKey = _configuration.GetSection("SendGrid:Key").Value;
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(apiKey, subject, message, toEmail);
        }

        private async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var fromEmail = _configuration.GetSection("SendGrid:Email").Value;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail), //Later change email or store it in secrets if we want to make the repo public
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}
