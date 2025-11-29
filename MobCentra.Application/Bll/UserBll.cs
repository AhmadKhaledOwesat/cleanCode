using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    public class UserBll(IBaseDal<Users, Guid, UserFilter> baseDal, IAuthenticationManager authenticationManager, ISettingBll settingBll, IEmailSender emailSender, IConstraintBll constraintBll, IUserGroupBll userGroupBll, IUserCommandBll userCommandBll, IDcpMapper dcpMapper, IUserRoleBll userRoleBll, ICompanyBll companyBll, ICompanySubscriptionBll companySubscriptionBll) : BaseBll<Users, Guid, UserFilter>(baseDal), IUserBll
    {
        public override async Task AddAsync(Users entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfUsers);
            entity.Password = entity.Password.HashedPassword();
            await base.AddAsync(entity);
        }
        public async Task<DcpResponse<UsersDto>> LoginAsync(string userName, string password, string companyCode)
        {
            Company company = await companyBll.FindByExpressionAsync(x => x.CompanyCode == companyCode);
            if (company == null || company.Active == 0) return new DcpResponse<UsersDto>(null, "الرجاء التاكد من رمز الشركة", false);
            Users user = await baseDal.FindByExpressionAsync(x => x.Password == password.HashedPassword() && x.UserName == userName && x.CompanyId == company.Id && x.Active == 1);
            if (user == null) return new DcpResponse<UsersDto>(null, "الرجاء التاكد من كلمة المرور ورمز المستخدم", false);
            List<CompanySubscription> companySubscriptions = await companySubscriptionBll.FindAllByExpressionAsync(x => x.CompanyId == company.Id);
            if (companySubscriptions is { Count: > 0 })
            {
                CompanySubscription companySubscription = companySubscriptions.LastOrDefault();
                if (companySubscription.ToDate < new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    return new DcpResponse<UsersDto>(null, "لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام", false);
            }
            UsersDto usersDto = dcpMapper.Map<UsersDto>(user);
            usersDto.Permssions = user.UserRoles.Where(a => a.Role != null && a.Role.Active == 1).SelectMany(a => a.Role?.RolePrivileges).Select(x => x.PrivilegeId).ToArray() ?? [];
            usersDto.EndDate = companySubscriptions.LastOrDefault()?.ToDate?.ToString("yyyy-MM-dd") ?? "";
            usersDto.CompanyName = company.NameAr;
            usersDto.CompanyNameOt = company.NameOt;
            usersDto.CompanyCoordination = company.CompanyCoordination;
            usersDto.Token = authenticationManager.GenerateToken(usersDto.FullName, usersDto.Id).Token;
            return new DcpResponse<UsersDto>(usersDto);
        }
        public async Task<DcpResponse<string>> ResetPasswordAsync(string userName, string companyCode)
        {
            Company company = await companyBll.FindByExpressionAsync(x => x.CompanyCode == companyCode);
            if (company == null || company.Active == 0) return new DcpResponse<string>(null, "الرجاء التاكد من رمز الشركة", false);
            Users user = await baseDal.FindByExpressionAsync(x => x.UserName == userName && x.CompanyId == company.Id && x.Active == 1);
            if (user == null) return new DcpResponse<string>(null, "الرجاء التاكد من رمز المستخدم", false);
            bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(company.Id);
            if (!isValidSubscription)
                return new DcpResponse<string>(null, "لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام", false);
            return await SendEamilAsync(user);
        }

        public async Task<DcpResponse<string>> UpdatePasswordAsync(Guid userId, string newPassword)
        {
            if (await GetByIdAsync(userId) is Users user && !newPassword.IsNullOrEmpty())
            {
                user.Password = newPassword.HashedPassword();
                await base.UpdateAsync(user);
                return new DcpResponse<string>("success");
            }
            return new DcpResponse<string>("User not found", "User not found", false);
        }

        private async Task<DcpResponse<string>> SendEamilAsync(Users user)
        {
            string link = "https://mobcentra.com/update-password/" + user.Id;
            string emailBody = $@"<!doctype html>
<html>
  <head>
    <meta charset=""utf-8""/>
    <meta name=""viewport"" content=""width=device-width,initial-scale=1""/>
  </head>
  <body style=""font-family: Arial, Helvetica, sans-serif; color: #222; margin:0; padding:20px; background:#f6f8fb;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
      <tr>
        <td align=""center"">
          <table width=""600"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""background:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 2px 6px rgba(0,0,0,0.08);"">
            <tr>
              <td style=""padding:24px; text-align:left;"">
                <h2 style=""margin:0 0 12px 0; font-size:20px;"">Hello {user.FullName},</h2>
                <p style=""margin:0 0 16px 0; line-height:1.5;"">
                  We received a request to reset your password for your MobCentra account. Click the button below to set a new password. This link will expire in 30 minutes.
                </p>

                <p style=""text-align:center; margin:20px 0;"">
                  <a href=""{link}"" style=""display:inline-block; padding:12px 20px; text-decoration:none; border-radius:6px; font-weight:600; border:1px solid #1a73e8;"">
                    Set your password
                  </a>
                </p>

                <p style=""margin:0 0 8px 0; font-size:13px; color:#555;"">
                  If the button doesn't work, copy and paste this URL into your browser:
                </p>
                <p style=""word-break:break-all; font-size:13px; color:#0066cc; margin:4px 0 16px 0;"">
                  {link}
                </p>

                <hr style=""border:none; border-top:1px solid #eee; margin:16px 0;""/>

                <p style=""font-size:12px; color:#777; margin:0 0 6px 0;"">
                  If you didn't request a password reset, you can ignore this email — no changes were made to your account.
                </p>
                <p style=""font-size:12px; color:#777; margin:0 0 12px 0;"">
                  Need help? Contact <a href=""mailto:info@mobcentra.com"" style=""color:#0066cc;"">info@mobcentra.com</a>
                </p>

                <p style=""font-size:13px; color:#999; margin:0;"">Thanks,<br/>Mobcentra – Centralizing Your Mobile World</p>
              </td>
            </tr>
            <tr>
              <td style=""background:#f2f4f7; padding:12px; font-size:12px; color:#999; text-align:center;"">
                © {DateTime.Now.Year}.  All rights reserved.
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>
";

            var toEmail = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.Notification.Email" && a.CompanyId == user.CompanyId);
            await emailSender.SendAsync("Password Reset", emailBody, toEmail.SettingValue);
            return new DcpResponse<string>("");
        }

        public override Task<PageResult<Users>> GetAllAsync(UserFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Users, bool>(a =>
                  (searchParameters.Keyword.IsNullOrEmpty() || a.FullName.Contains(searchParameters?.Keyword))
                && a.CompanyId == searchParameters.CompanyId);
            }

            return base.GetAllAsync(searchParameters);
        }

        public override async Task UpdateAsync(Users entity)
        {
            if (!string.IsNullOrEmpty(entity.NewPassword))
                entity.Password = entity.NewPassword.HashedPassword();
            if (entity.OperationType.HasFlag(Domain.Enum.OperationType.UserRole))
                await HandleUserRoles(entity);
            if (entity.OperationType.HasFlag(Domain.Enum.OperationType.UserCommand))
                await HandleUserCommands(entity);
            if (entity.OperationType.HasFlag(Domain.Enum.OperationType.UserGroup))
                await HandleUserGroups(entity);
            await base.UpdateAsync(entity);
        }

        private async Task HandleUserCommands(Users entity)
        {
            Expression<Func<UserCommand, bool>> expression = x => x.UserId == entity.Id;
            List<UserCommand> userCommands = await userCommandBll.FindAllByExpressionAsync(expression);
            if (userCommands.Count > 0)
                await userCommandBll.DeleteRangeAsync(userCommands);
            foreach (var item in entity.UserCommands)
            {
                item.GoogleCommand = null;
            }
            await userCommandBll.AddRangeAsync([.. entity.UserCommands]);
        }
        private async Task HandleUserGroups(Users entity)
        {
            Expression<Func<UserGroup, bool>> expression = x => x.UserId == entity.Id;
            List<UserGroup> userGroups = await userGroupBll.FindAllByExpressionAsync(expression);
            if (userGroups.Count > 0)
                await userGroupBll.DeleteRangeAsync(userGroups);
            foreach (var item in entity.UserGroups)
            {
                item.Group = null;
            }
            await userGroupBll.AddRangeAsync([.. entity.UserGroups]);
        }

        private async Task HandleUserRoles(Users entity)
        {
            Expression<Func<UserRole, bool>> expression = x => x.UserId == entity.Id;
            List<UserRole> userRoles = await userRoleBll.FindAllByExpressionAsync(expression);
            if (userRoles.Count > 0)
                await userRoleBll.DeleteRangeAsync(userRoles);
            await userRoleBll.AddRangeAsync([.. entity.UserRoles]);
        }
    }
}
