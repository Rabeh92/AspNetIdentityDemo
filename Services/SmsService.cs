using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AspNetIdentityDemo.Services
{
    public class SmsService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var sid = ConfigurationManager.AppSettings["twilio:sid"];
            var token = ConfigurationManager.AppSettings["twilio:token"];
            var from = ConfigurationManager.AppSettings["twilio:from"];

            //Configure Twilio
            TwilioClient.Init(sid, token);
            await MessageResource.CreateAsync(new PhoneNumber(message.Destination), from: from, 
                body: message.Body);

            
        }
    }
}