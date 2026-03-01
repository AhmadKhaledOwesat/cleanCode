using MobCentra.Application.Bll;
using MobCentra.Application.Dal;
using MobCentra.Application.Interfaces;
using MobCentra.Application.Mapper;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.EfContext;
using MobCentra.Infrastructure.Repositories;
using MobCentra.Notification.Bll;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace MobCentra.Application.DI
{
    public static class ServiceExtension
    {
        public static void AddDcpMapper(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<DcpMapper>();
            });

        }

        public static void AddEfDbContext(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.AddDbContext<StudioContext>(options => options.UseLazyLoadingProxies().UseSqlServer(
                configuration.GetConnectionString("MobCentra"),
                 x => x.UseNetTopologySuite()));
        }

        public static void AddServices(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<IDcpMapper, DcpMapper>();
            serviceDescriptors.AddScoped<IUserBll, UserBll>();
            serviceDescriptors.AddScoped(provider =>
            {
                return new Lazy<IUserBll>(() => provider.GetRequiredService<IUserBll>());
            });
            serviceDescriptors.AddScoped(provider =>
            {
                return new Lazy<IDeviceApplicationBll>(() => provider.GetRequiredService<IDeviceApplicationBll>());
            });
            serviceDescriptors.AddScoped(provider =>
            {
                return new Lazy<IGroupBll>(() => provider.GetRequiredService<IGroupBll>());
            });
            serviceDescriptors.AddScoped(provider =>
            {
                return new Lazy<IDeviceBll>(() => provider.GetRequiredService<IDeviceBll>());
            });
            serviceDescriptors.AddScoped(provider =>
            {
                return new Lazy<IProfileBll>(() => provider.GetRequiredService<IProfileBll>());
            });
            serviceDescriptors.AddScoped(provider =>
            {
                return new Lazy<ICompanyBll>(() => provider.GetRequiredService<ICompanyBll>());
            });
            serviceDescriptors.AddScoped(typeof(IIdentityManager<>), typeof(IdentityManager<>));
            serviceDescriptors.AddScoped<IAuthorizationChecker, AuthorizationChecker>();
            serviceDescriptors.AddScoped<IAuthenticationManager, AuthenticationManager>();
            serviceDescriptors.AddScoped<IDeviceBatteryTransBll, DeviceBatteryTransBll>();
            serviceDescriptors.AddScoped<IDeviceQueuBll, DeviceQueuBll>();
            serviceDescriptors.AddScoped<IDeviceUsageBll, DeviceUsageBll>();
            serviceDescriptors.AddScoped<ITaskStatusBll, TaskStatusBll>();
            serviceDescriptors.AddScoped<IAppUserBll, AppUserBll>();
            serviceDescriptors.AddScoped<ICompanyBll, CompanyBll>();
            serviceDescriptors.AddScoped<ITransactionBll, TransactionBll>();
            serviceDescriptors.AddScoped<IGeoFencSettingBll, GeoFencSettingBll>();
            serviceDescriptors.AddScoped<ITasksBll, TasksBll>();
            serviceDescriptors.AddScoped<ICityBll, CityBll>();
            serviceDescriptors.AddScoped<IDeviceBll, DeviceBll>();
            serviceDescriptors.AddScoped<IMDMCommandBll, MDMCommandBll>();
            serviceDescriptors.AddScoped<IGeoFencBll, GeoFencBll>();
            serviceDescriptors.AddScoped<IBlackListAppBll, BlackListAppBll>();
            serviceDescriptors.AddScoped<IGroupBll, GroupBll>();
            serviceDescriptors.AddScoped<ICommandGroupBll, CommandGroupBll>();
            serviceDescriptors.AddScoped<IEmailSender, EmailSender>();
            serviceDescriptors.AddScoped<IDeviceStorageFileBll, DeviceStorageFileBll>();
            serviceDescriptors.AddScoped<ICompanySubscriptionBll, CompanySubscriptionBll>();
            serviceDescriptors.AddScoped<ISettingBll, SettingBll>();
            serviceDescriptors.AddScoped<IRoleBll, RoleBll>();
            serviceDescriptors.AddScoped<IPrivilegeBll, PrivilegeBll>();
            serviceDescriptors.AddScoped<IDeviceFileBll, DeviceFileBll>();
            serviceDescriptors.AddScoped<IUserRoleBll, UserRoleBll>();
            serviceDescriptors.AddScoped<IVersionBll, VersionBll>();
            serviceDescriptors.AddScoped<IUserCommandBll, UserCommandBll>();
            serviceDescriptors.AddScoped<IUserGroupBll, UserGroupBll>();
            serviceDescriptors.AddScoped<IApplicationBll, ApplicationBll>();
            serviceDescriptors.AddScoped<IDeviceNotificationBll, DeviceNotificationBll>();
            serviceDescriptors.AddScoped<IRolePrivilegeBll, RolePrivilegeBll>();
            serviceDescriptors.AddScoped<IReportBll, ReportBll>();
            serviceDescriptors.AddScoped<IDeviceTransactionBll, DeviceTransactionBll>();
            serviceDescriptors.AddScoped<IDeviceLogBll, DeviceLogBll>();
            serviceDescriptors.AddScoped<IDeviceApplicationBll, DeviceApplicationBll>();
            serviceDescriptors.AddScoped<IReportParameterBll, ReportParameterBll>();
            serviceDescriptors.AddScoped<IDevicesGeoFenceLogBll, DevicesGeoFenceLogBll>();
            serviceDescriptors.AddScoped<IStatisticBll, StatisticBll>();
            serviceDescriptors.AddScoped<IFeatureBll, FeatureBll>();
            serviceDescriptors.AddScoped<IProfileBll, ProfileBll>();
            serviceDescriptors.AddScoped<IProfileFeatureBll, ProfileFeatureBll>();
            serviceDescriptors.AddScoped<IConstraintBll, ConstraintBll>();
            serviceDescriptors.AddScoped<INotificationBll, NotificationBll>();
            serviceDescriptors.AddScoped(typeof(IBaseBll<,,>), typeof(BaseBll<,,>));
            serviceDescriptors.AddScoped(typeof(IBaseDal<,,>), typeof(BaseDal<,,>));
            serviceDescriptors.AddScoped(typeof(IEfRepository<,>), typeof(EfRepository<,>));
        }
    }
}
