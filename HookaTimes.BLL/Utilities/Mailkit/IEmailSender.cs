using HookaTimes.BLL.Utilities.MailKit;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Utilities.Mailkit
{
    public interface IEmailSender
    {
        MailKitEmailSenderOptions Options { get; set; }

        Task<bool> Execute(string to, string subject, string message);
        Task<bool> SendEmailAsync(string email, string subject, string htmlMessage);
    }
}