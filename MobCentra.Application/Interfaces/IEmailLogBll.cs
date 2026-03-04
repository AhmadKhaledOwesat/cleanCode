using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Interfaces
{
    public interface IEmailLogBll : IBaseBll<EmailLog, Guid, EmailLogFilter>
    {
    }
}
