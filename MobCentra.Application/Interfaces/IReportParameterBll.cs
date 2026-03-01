using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IReportParameterBll : IBaseBll<ReportParameter, Guid, ReportParameterFilter>
    {
        Task<dynamic> GetListDataByIdAsync(Guid id, Guid companyId);
    }
}
