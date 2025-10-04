using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface ICompanyBll : IBaseBll<Company, Guid, CompanyFilter>
    {
        Task<DcpResponse<CompanyDto>> LoginAsync(string userName, string password, string companyCode);
    }
}
