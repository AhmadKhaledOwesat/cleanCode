namespace MobCentra.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task<string> SendAsync(string subject, string body,string to);
    }
}
