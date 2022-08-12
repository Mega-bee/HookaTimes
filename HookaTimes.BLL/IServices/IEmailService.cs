namespace HookaTimes.BLL.IServices
{
    public interface IEmailSender
    {
        void Send(string to, string subject, string html, string from = null);
    }
}
