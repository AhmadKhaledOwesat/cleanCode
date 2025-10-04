using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface ICompanyBll : IBaseBll<Company, Guid, CompanyFilter>
    {
        Task<DcpResponse<CompanyDto>> LoginAsync(string userName, string password, string companyCode);
    }
}
