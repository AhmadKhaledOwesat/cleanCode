using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for email log operations
    /// </summary>
    public class EmailLogBll(IBaseDal<EmailLog, Guid, EmailLogFilter> baseDal) : BaseBll<EmailLog, Guid, EmailLogFilter>(baseDal), IEmailLogBll
    {
        /// <summary>
        /// Retrieves email logs with optional filtering by company, send status, and keyword
        /// </summary>
    }
}
