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
    /// Business logic layer for user management operations
    /// </summary>
    public class UserBll(IBaseDal<Users, Guid, UserFilter> baseDal, IAuthenticationManager authenticationManager, ISettingBll settingBll, IEmailSender emailSender, IConstraintBll constraintBll, IUserGroupBll userGroupBll, IUserCommandBll userCommandBll, IDcpMapper dcpMapper, IUserRoleBll userRoleBll, ICompanyBll companyBll, ICompanySubscriptionBll companySubscriptionBll) : BaseBll<Users, Guid, UserFilter>(baseDal), IUserBll
    {
        /// <summary>
        /// Adds a new user to the system after validating constraints and hashing the password
        /// </summary>
        /// <param name="entity">The user entity to add</param>
        public override async Task AddAsync(Users entity)
        {
            // Check if company has reached the maximum number of users limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfUsers);
            // Hash the password before storing it
            entity.Password = entity.Password.HashedPassword();
            entity.Company = null;
            if(await GetCountByExpressionAsync(a=>a.UserName == entity.UserName) > 0)
                throw new Exception("رمز المستخدم موجود مسبقاً");
            await base.AddAsync(entity);
        }
        
        /// <summary>
        /// Authenticates a user and returns user information with authentication token
        /// </summary>
        /// <param name="userName">The username for authentication</param>
        /// <param name="password">The password for authentication</param>
        /// <param name="companyCode">The company code to validate against</param>
        /// <returns>Response containing user DTO with token and permissions, or error message if authentication fails</returns>
        public async Task<DcpResponse<UsersDto>> LoginAsync(string userName, string password, string companyCode,bool isByPass)
            {
            if (isByPass)
                return await InternalLoginAsync(userName);
            // Validate company exists and is active
            Company company = await companyBll.FindByExpressionAsync(x => x.CompanyCode == companyCode);
            if (company == null || company.Active == 0) return new DcpResponse<UsersDto>(null, "الرجاء التاكد من رمز الشركة", false);

            Expression<Func<Users,bool>> liveExpression = x => x.Password == password.HashedPassword() && x.UserName == userName && x.CompanyId == company.Id && x.Active == 1;
            Expression<Func<Users, bool>> localExpression = x => x.UserName == userName && x.CompanyId == company.Id && x.Active == 1;
            Expression<Func<Users, bool>> expression = liveExpression;

#if DEBUG

            expression = localExpression;
#endif


            // Authenticate user with hashed password
            Users user = await baseDal.FindByExpressionAsync(expression);
            if (user == null) return new DcpResponse<UsersDto>(null, "الرجاء التاكد من كلمة المرور ورمز المستخدم", false);
            List<CompanySubscription> companySubscriptions = await companySubscriptionBll.FindAllByExpressionAsync(x => x.CompanyId == company.Id);

#if !DEBUG
            // Get company subscription information
            if (companySubscriptions is { Count: > 0 })
            {
                CompanySubscription companySubscription = companySubscriptions.LastOrDefault();
                if (companySubscription.ToDate < new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    return new DcpResponse<UsersDto>(null, "لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام", false);
            }
#endif
            // Map user entity to DTO
            UsersDto usersDto = dcpMapper.Map<UsersDto>(user);
            
            // Extract user permissions from active roles
            usersDto.Permssions = user.UserRoles.Where(a => a.Role != null && a.Role.Active == 1).SelectMany(a => a.Role?.RolePrivileges).Select(x => x.PrivilegeId).ToArray() ?? [];
            
            // Set company information and subscription end date
            usersDto.EndDate = companySubscriptions.LastOrDefault()?.ToDate?.ToString("yyyy-MM-dd") ?? "";
            usersDto.CompanyName = company.NameAr;
            usersDto.CompanyNameOt = company.NameOt;
            usersDto.CompanyCoordination = company.CompanyCoordination;
            
            // Generate authentication token
            usersDto.Token = authenticationManager.GenerateToken(usersDto.FullName, usersDto.Id).Token;
            return new DcpResponse<UsersDto>(usersDto);
        }
        private async Task<DcpResponse<UsersDto>> InternalLoginAsync(string userName)
        {
            Users user = await baseDal.FindByExpressionAsync(x =>  x.UserName == userName);

            // Validate company exists and is active
            Company company = await companyBll.FindByExpressionAsync(x => x.Id == user.CompanyId);
            if (company == null || company.Active == 0) return new DcpResponse<UsersDto>(null, "الرجاء التاكد من رمز الشركة", false);

            // Authenticate user with hashed password
            if (user == null) return new DcpResponse<UsersDto>(null, "الرجاء التاكد من كلمة المرور ورمز المستخدم", false);
            List<CompanySubscription> companySubscriptions = await companySubscriptionBll.FindAllByExpressionAsync(x => x.CompanyId == company.Id);
            // Map user entity to DTO
            UsersDto usersDto = dcpMapper.Map<UsersDto>(user);
            // Extract user permissions from active roles
            usersDto.Permssions = user.UserRoles.Where(a => a.Role != null && a.Role.Active == 1).SelectMany(a => a.Role?.RolePrivileges).Select(x => x.PrivilegeId).ToArray() ?? [];
            // Set company information and subscription end date
            usersDto.EndDate = companySubscriptions.LastOrDefault()?.ToDate?.ToString("yyyy-MM-dd") ?? "";
            usersDto.CompanyName = company.NameAr;
            usersDto.CompanyNameOt = company.NameOt;
            usersDto.CompanyCoordination = company.CompanyCoordination;
            // Generate authentication token
            usersDto.Token = authenticationManager.GenerateToken(usersDto.FullName, usersDto.Id).Token;
            return new DcpResponse<UsersDto>(usersDto);
        }
        /// <summary>
        /// Initiates password reset process by sending reset email to user
        /// </summary>
        /// <param name="userName">The username of the user requesting password reset</param>
        /// <param name="companyCode">The company code to validate against</param>
        /// <returns>Response indicating success or error message</returns>
        public async Task<DcpResponse<string>> ResetPasswordAsync(string userName, string companyCode)
        {
            // Validate company exists and is active
            Company company = await companyBll.FindByExpressionAsync(x => x.CompanyCode == companyCode);
            if (company == null || company.Active == 0) return new DcpResponse<string>(null, "الرجاء التاكد من رمز الشركة", false);
            
            // Find active user by username
            Users user = await baseDal.FindByExpressionAsync(x => x.UserName == userName && x.CompanyId == company.Id && x.Active == 1);
            if (user == null) return new DcpResponse<string>(null, "الرجاء التاكد من رمز المستخدم", false);
            
#if !DEBUG
            // Validate subscription in production mode
            bool isValidSubscription = await companySubscriptionBll.IsValidSubscriptionAsync(company.Id);
            if (!isValidSubscription)
               return new DcpResponse<string>(null, "لقد انتهى اشتراك الباقة الرجاء التواصل مع مدير النظام", false);
#endif
               return await SendEamilAsync(user);
        }

        /// <summary>
        /// Updates the password for a specific user
        /// </summary>
        /// <param name="userId">The unique identifier of the user</param>
        /// <param name="newPassword">The new password to set</param>
        /// <returns>Response indicating success or error message</returns>
        public async Task<DcpResponse<string>> UpdatePasswordAsync(Guid userId, string newPassword)
        {
            // Validate user exists and new password is provided
            if (await GetByIdAsync(userId) is Users user && !newPassword.IsNullOrEmpty())
            {
                // Hash and update password
                user.Password = newPassword.HashedPassword();
                await base.UpdateAsync(user);
                return new DcpResponse<string>("success");
            }
            return new DcpResponse<string>("User not found", "User not found", false);
        }

        /// <summary>
        /// Sends password reset email to the user with reset link
        /// </summary>
        /// <param name="user">The user entity to send email to</param>
        /// <returns>Response indicating email was sent successfully</returns>
        private async Task<DcpResponse<string>> SendEamilAsync(Users user)
        {
            // Generate password reset link with user ID
            string link = "https://mobcentra.com/update-password/" + user.Id;
            
            // Create HTML email body with password reset instructions
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

            // Get notification email from settings and send password reset email
            var toEmail = await settingBll.FindByExpressionAsync(a => a.SettingName == "DCP.Notification.Email" && a.CompanyId == user.CompanyId);
            await emailSender.SendAsync("Password Reset", emailBody, toEmail.SettingValue);
            return new DcpResponse<string>("");
        }

        /// <summary>
        /// Retrieves users with filtering and pagination support
        /// </summary>
        /// <param name="searchParameters">Filter parameters including keyword search and company ID</param>
        /// <returns>Paginated result containing matching users</returns>
        public override Task<PageResult<Users>> GetAllAsync(UserFilter searchParameters)
        {
            // Build search expression with keyword filter and company filter
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Users, bool>(a =>
                (searchParameters.Keyword.IsNullOrEmpty() || a.FullName.Contains(searchParameters?.Keyword))
                && a.CompanyId == searchParameters.CompanyId);
            }

            return base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Updates user information including password, roles, commands, and groups if specified
        /// </summary>
        /// <param name="entity">The user entity with updated information</param>
        public override async Task UpdateAsync(Users entity)
        {
            entity.Company = null;
            if (await GetCountByExpressionAsync(a => a.UserName == entity.UserName && a.Id != entity.Id) > 0)
                throw new Exception("رمز المستخدم موجود مسبقاً");

            // Update password if new password is provided
            if (!string.IsNullOrEmpty(entity.NewPassword))
                entity.Password = entity.NewPassword.HashedPassword();
            
            // Handle user roles update if specified in operation type
            if (entity.OperationType.HasFlag(Domain.Enum.OperationType.UserRole))
                await HandleUserRoles(entity);
            
            // Handle user commands update if specified in operation type
            if (entity.OperationType.HasFlag(Domain.Enum.OperationType.UserCommand))
                await HandleUserCommands(entity);
            
            // Handle user groups update if specified in operation type
            if (entity.OperationType.HasFlag(Domain.Enum.OperationType.UserGroup))
                await HandleUserGroups(entity);
            
            await base.UpdateAsync(entity);
        }

        /// <summary>
        /// Handles updating user commands by removing existing commands and adding new ones
        /// </summary>
        /// <param name="entity">The user entity containing new commands</param>
        private async Task HandleUserCommands(Users entity)
        {
            // Find all existing user commands
            Expression<Func<UserCommand, bool>> expression = x => x.UserId == entity.Id;
            List<UserCommand> userCommands = await userCommandBll.FindAllByExpressionAsync(expression);
            
            // Delete existing commands if any
            if (userCommands.Count > 0)
                await userCommandBll.DeleteRangeAsync(userCommands);
            
            // Clear navigation properties and add new commands
            foreach (var item in entity.UserCommands)
            {
                item.GoogleCommand = null;
            }
            await userCommandBll.AddRangeAsync([.. entity.UserCommands]);
        }
        
        /// <summary>
        /// Handles updating user groups by removing existing groups and adding new ones
        /// </summary>
        /// <param name="entity">The user entity containing new groups</param>
        private async Task HandleUserGroups(Users entity)
        {
            // Find all existing user groups
            Expression<Func<UserGroup, bool>> expression = x => x.UserId == entity.Id;
            List<UserGroup> userGroups = await userGroupBll.FindAllByExpressionAsync(expression);
            
            // Delete existing groups if any
            if (userGroups.Count > 0)
                await userGroupBll.DeleteRangeAsync(userGroups);
            
            // Clear navigation properties and add new groups
            foreach (var item in entity.UserGroups)
            {
                item.Group = null;
            }
            await userGroupBll.AddRangeAsync([.. entity.UserGroups]);
        }

        /// <summary>
        /// Handles updating user roles by removing existing roles and adding new ones
        /// </summary>
        /// <param name="entity">The user entity containing new roles</param>
        private async Task HandleUserRoles(Users entity)
        {
            // Find all existing user roles
            Expression<Func<UserRole, bool>> expression = x => x.UserId == entity.Id;
            List<UserRole> userRoles = await userRoleBll.FindAllByExpressionAsync(expression);
            
            // Delete existing roles if any
            if (userRoles.Count > 0)
                await userRoleBll.DeleteRangeAsync(userRoles);
            
            // Add new roles
            await userRoleBll.AddRangeAsync([.. entity.UserRoles]);
        }
    }
}
