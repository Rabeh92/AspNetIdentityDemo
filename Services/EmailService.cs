using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using System.Configuration;
using SendGrid.Helpers.Mail;

namespace AspNetIdentityDemo.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var client = new SendGridClient(ConfigurationManager.AppSettings["sendGrid:key"]);
            var from = new EmailAddress("rabeh@c17e.com");
            var to = new EmailAddress(message.Destination);
            var msg = MailHelper.CreateSingleEmail(from, to, message.Subject, message.Body, message.Body);
            await client.SendEmailAsync(msg);
        }
    }
}