// GYMNETIC.Core/Services/EmailSender.cs
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GYMNETIC.Core.Services
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string ApplicationUrl { get; set; } = string.Empty;
    }

    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                Console.WriteLine($"Sending email to {email} with subject '{subject}' and message '{message}'");

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
                
                Console.WriteLine($"Email sent successfully to {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
                throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
            }
        }
    }
}