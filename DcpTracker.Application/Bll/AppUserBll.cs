using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    public class AppUserBll(IBaseDal<AppUsers, Guid, AppUserFilter> baseDal,ICompanyBll companyBll,IDcpMapper dcpMapper) : BaseBll<AppUsers, Guid, AppUserFilter>(baseDal), IAppUserBll
    {

        public async Task<DcpResponse<AppUserDto>> LoginAsync(LoginDto loginDto)
        {
            Expression<Func<AppUsers, bool>> expression = x => x.Password == loginDto.Paswword.HashedPassword() && x.UserName == loginDto.UserName;
            AppUsers user = await baseDal.FindByExpressionAsync(expression);
            if (user == null) return new DcpResponse<AppUserDto>(null, "Please check user name and password", false);
            Expression<Func<Company, bool>> companyExpression = x => x.CompanyCode == loginDto.CompanyCode;
            Company company = await companyBll.FindByExpressionAsync(companyExpression);
            if (company == null) return new DcpResponse<AppUserDto>(null, "Please check company code", false);
            user.PushNotificationToken = loginDto.Token;
            AppUserDto usersDto = dcpMapper.Map<AppUserDto>(user);
            await base.UpdateAsync(user);
            usersDto.Settings = dcpMapper.Map<List<SettingDto>>(company.Settings);
            return new DcpResponse<AppUserDto>(usersDto);
        }
        public override async Task AddAsync(AppUsers entity)
        {
            entity.Password = entity.Password.HashedPassword();
            await base.AddAsync(entity);
        }
        public override Task<PageResult<AppUsers>> GetAllAsync(AppUserFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                if (!searchParameters.FullName.IsNullOrEmpty())
                    searchParameters.Expression = new Func<AppUsers, bool>(a => a.FullName == searchParameters?.FullName);
            }

            return base.GetAllAsync(searchParameters);
        }

        public override async Task UpdateAsync(AppUsers entity)
        {
            await base.UpdateAsync(entity);
        }
    }
}
