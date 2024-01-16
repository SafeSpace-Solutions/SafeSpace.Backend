using Microsoft.Extensions.Configuration;
using SafeSpace.Application.DTOs.Account;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;

namespace SafeSpace.Application.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailAsync(EmailSendDto emailSend)
        {
            var client = new MailjetClient(_config["MailJet:ApiKey"], _config["MailJet:SecretKey"]);
            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_config["Email:From"], _config["Email:ApplicationName"]))
                .WithSubject(emailSend.Subject)
                .WithHtmlPart(emailSend.Body)
                .WithTo(new SendContact(emailSend.To))
                .Build();
            var respone = await client.SendTransactionalEmailAsync(email);
            if (respone.Messages != null)
            {
                if (respone.Messages[0].Status == "success")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
