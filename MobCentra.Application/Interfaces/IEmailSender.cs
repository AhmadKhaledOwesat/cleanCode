﻿namespace MobCentra.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(string subject, string body,string to);
    }
}
