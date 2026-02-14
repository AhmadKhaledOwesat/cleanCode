using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MobCentra.Domain.Interfaces;
using System.Security.Cryptography.X509Certificates;
namespace MobCentra.Application.Bll
{
    public  class EmailOptions
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public SmtpOptions Smtp { get; set; }
    }
    public  class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseStartTls { get; set; }
        public string User { get; set; } 
        public string Password { get; set; } 
    }
    public sealed class EmailSender : IEmailSender
    {
        private readonly EmailOptions _options;
        public EmailSender(IOptions<EmailOptions> options)
        => _options = options.Value;


        public async Task SendAsync(string subject, string body,string to)
        {
            MimeMessage mimeMessage = new();
            mimeMessage.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
            mimeMessage.To.Add(new MailboxAddress("Client", to));
            mimeMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
            {
                if (certificate is X509Certificate2 cert)
                {
                    return cert.Subject.Contains("smtp.a.cloudfilter.net");
                }
                return false;
            };
            await client.ConnectAsync(_options.Smtp.Host, _options.Smtp.Port,
            _options.Smtp.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);

            if (!string.IsNullOrWhiteSpace(_options.Smtp.User))
                await client.AuthenticateAsync(_options.Smtp.User, _options.Smtp.Password);

            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
