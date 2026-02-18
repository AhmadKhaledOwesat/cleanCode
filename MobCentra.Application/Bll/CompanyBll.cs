using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for company management operations
    /// </summary>
    public class CompanyBll(IBaseDal<Company,Guid,CompanyFilter> baseDal,IDcpMapper dcpMapper,IDeviceBll deviceBll,Lazy<IUserBll> userBll,IAuthenticationManager authenticationManager) : BaseBll<Company,Guid,CompanyFilter>(baseDal) , ICompanyBll
    {
        /// <summary>
        /// Authenticates a company login by validating company code, user credentials, and device limits
        /// </summary>
        /// <param name="userName">The username for authentication</param>
        /// <param name="password">The password for authentication</param>
        /// <param name="companyCode">The company code to validate</param>
        /// <returns>Response containing company DTO if authentication succeeds, or error message if it fails</returns>
        public async Task<DcpResponse<CompanyDto>> LoginAsync(string userName, string password, string companyCode)
        {
            // Validate company code
            Expression<Func<Company, bool>> expression = x=> x.CompanyCode == companyCode;
            Company company = await baseDal.FindByExpressionAsync(expression);
            if (company == null) return new DcpResponse<CompanyDto>(null,"Please check company code", false);
            
            // Authenticate user with hashed password
            Expression<Func<Users, bool>> loginExpression = x => x.UserName == userName && x.Password == password.HashedPassword() && x.CompanyId ==  company.Id;
            Users users = await userBll.Value.FindByExpressionAsync(loginExpression);
            if (users == null) return new DcpResponse<CompanyDto>(null, "Please check user name and password", false);
            
            // Check device limit
            Expression<Func<Device, bool>> cntExpression = x => x.CompanyId == company.Id;
            int count = await deviceBll.GetCountByExpressionAsync(cntExpression);
            if(count > company.NoOfDevices + 1) return new DcpResponse<CompanyDto>(null, "Company has reach device limit", false);
            
            // Map company to DTO and return
            CompanyDto usersDto = dcpMapper.Map<CompanyDto>(company);
            Tokens tokens = authenticationManager.GenerateToken(userName, users.Id);
            usersDto.Token = tokens.Token;
            usersDto.RefreshToken = tokens.RefreshToken;

            return new DcpResponse<CompanyDto>(usersDto);
        }
        
        /// <summary>
        /// Retrieves companies with filtering by description (name in Arabic) and active status
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching companies</returns>
        public override Task<PageResult<Company>> GetAllAsync(CompanyFilter searchParameters)
        {
            // Build search expression with description and active status filters
            if(searchParameters is not null)
            {
                if(!searchParameters.Description.IsNullOrEmpty())
                    searchParameters.Expression = new Func<Company, bool>(a => a.NameAr.Contains(searchParameters.Description) && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
