using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IReportParameterBll : IBaseBll<ReportParameter, Guid, ReportParameterFilter>
    {
    }
}
