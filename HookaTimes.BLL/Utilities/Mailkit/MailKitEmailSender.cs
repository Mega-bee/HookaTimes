using HookaTimes.BLL.Utilities.MailKit;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Utilities.Mailkit
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<MailKitEmailSenderOptions> options)
        {
            this.Options = options.Value;
        }
        public MailKitEmailSenderOptions Options { get; set; }


        public async Task<bool> SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return await Execute(email, subject, htmlMessage);
        }

        public async Task<bool> Execute(string to, string subject, string message)
        {
            // create message
            //var email = new MimeMessage();

            //if (!string.IsNullOrEmpty(Options.Sender_Name))
            //    email.Sender.Name = Options.Sender_Name;
            //email.From.Add(MailboxAddress.Parse(Options.Sender_EMail));
            //email.To.Add(MailboxAddress.Parse(to));
            //email.Subject = subject;
            ////email.Body = new TextPart(TextFormat.Html);
            //Header templateHadi = new Header("X-MJ-TemplateID", "4139941");
            //Header templateEngine = new Header("X-Mj-TemplateLanguage", 1);
            //var obj = new
            //{
            //    OTP = message,
            //};
            //List<dynamic> variables = new List<dynamic>()
            //{
            //    obj,
            //};
            //string json = JsonSerializer.Serialize(variables);

            //Header variable = new Header("X-MJ-Vars", json);
            //email.Headers.Add(templateHadi);
            //email.Headers.Add(variable);
            //email.Headers.Add(templateEngine);


            //// send email
            //using (var smtp = new SmtpClient())
            //{
            //    smtp.Connect(Options.Host_Address, Options.Host_Port, SecureSocketOptions.StartTls);
            //    smtp.Authenticate(Options.Host_Username, Options.Host_Password);
            //    smtp.Send(email);
            //    smtp.Disconnect(true);
            //}

            //return Task.FromResult(true);
            MailjetClient client = new MailjetClient(Options.Host_Username, Options.Host_Password);



            MailjetRequest request = new MailjetRequest
            {
                Resource = SendV31.Resource,
            }
               .Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", Options.Sender_EMail},
                  {"Name", Options.Sender_Name}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", to},
                   {"Name", to}
                   }
                  }},
                  {"TemplateID", 4139941},
                 {"TemplateLanguage", true},
                 {"Subject", subject},
                 {"Variables", new JObject {
                  {"OTP", message},
                  }}
                 }
                   });
            MailjetResponse response = await client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}