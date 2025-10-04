﻿namespace DcpTracker.Application.Interfaces
{
    public interface IIdentityManager<TId> where TId : struct
    {
        TId CurrentUserId { get;}
    }
}
