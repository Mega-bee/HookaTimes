using HookaTimes.BLL.Utilities.MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Utilities.Mailkit
{
    public class MailKitEmailSender : IEmailSender
    {
        public MailKitEmailSender(IOptions<MailKitEmailSenderOptions> options)
        {
            this.Options = options.Value;
        }
        public MailKitEmailSenderOptions Options { get; set; }


        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public Task Execute(string to, string subject, string message)
        {
            // create message
            var email = new MimeMessage();

            //if (!string.IsNullOrEmpty(Options.Sender_Name))
            //    email.Sender.Name = Options.Sender_Name;
            email.From.Add(MailboxAddress.Parse(Options.Sender_EMail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            //email.Body = new TextPart(TextFormat.Html);
            Header templateHadi = new Header("X-MJ-TemplateID", "4139941");
            var obj = new
            {
                OTP = message,
            };
            email.Headers.Add(templateHadi);

            // send email
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(Options.Host_Address, Options.Host_Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Options.Host_Username, Options.Host_Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            return Task.FromResult(true);
        }
    }
}