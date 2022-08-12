using HookaTimes.BLL.IServices;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;

namespace HookaTimes.BLL.Service
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string to, string subject, string html, string from = null)
        {

            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from != null ? from : "hadi.bawarshi@mega-bee.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };
            try
            {



                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("in-v3.mailjet.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("bbd22541cdaac42d5483d981a5164fd9", "f9a44ea47380cb678da383f4d2670e3f");
                smtp.Send(email);
                smtp.Disconnect(true);
                smtp.Dispose();
                //smtp.MessageSent += OnMessageSent;
            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}
