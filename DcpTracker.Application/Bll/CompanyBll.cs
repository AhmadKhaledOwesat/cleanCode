using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    public class CompanyBll(IBaseDal<Company,Guid,CompanyFilter> baseDal,IDcpMapper dcpMapper,IDeviceBll deviceBll,Lazy<IUserBll> userBll) : BaseBll<Company,Guid,CompanyFilter>(baseDal) , ICompanyBll
    {
        public async Task<DcpResponse<CompanyDto>> LoginAsync(string userName, string password, string companyCode)
        {
            Expression<Func<Company, bool>> expression = x=> x.CompanyCode == companyCode;
            Company company = await baseDal.FindByExpressionAsync(expression);
            if (company == null) return new DcpResponse<CompanyDto>(null,"Please check company code", false);
            Expression<Func<Users, bool>> loginExpression = x => x.UserName == userName && x.Password == password.HashedPassword() && x.CompanyId ==  company.Id;
            Users users = await userBll.Value.FindByExpressionAsync(loginExpression);
            if (users == null) return new DcpResponse<CompanyDto>(null, "Please check user name and password", false);
            Expression<Func<Device, bool>> cntExpression = x => x.CompanyId == company.Id;
            int count = await deviceBll.GetCountByExpressionAsync(cntExpression);
            if(count > company.NoOfDevices + 1) return new DcpResponse<CompanyDto>(null, "Company has reach device limit", false);
            CompanyDto usersDto = dcpMapper.Map<CompanyDto>(company);
            return new DcpResponse<CompanyDto>(usersDto);
        }
        public override Task<PageResult<Company>> GetAllAsync(CompanyFilter searchParameters)
        {
            if(searchParameters is not null)
            {
                if(!searchParameters.Description.IsNullOrEmpty())
                    searchParameters.Expression = new Func<Company, bool>(a => a.NameAr.Contains(searchParameters.Description) && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
