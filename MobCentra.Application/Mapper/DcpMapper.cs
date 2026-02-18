
using AutoMapper;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using NetTopologySuite.Geometries;
using Profile = AutoMapper.Profile;
using TaskStatus = MobCentra.Domain.Entities.TaskStatus;

namespace MobCentra.Application.Mapper
{
    public class DcpMapper : Profile, IDcpMapper
    {
        private readonly IMapper _mapper;
        public DcpMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public DcpMapper()
        {
            CreateMap<DevicesGeoFenceLog, DevicesGeoFenceLogDto>().ForMember(dest => dest.Coordinations, src => src.MapFrom(a => a.Coordinations == null ? string.Empty : $"{a.Coordinations.X},{a.Coordinations.Y}"));
            CreateMap<Users, UsersDto>().ReverseMap();
            CreateMap<City, CityDto>()
                .ForMember(dest => dest.CreatedByName, src => src.MapFrom(a => a.User == null ? "مدير النظام" : a.User.FullName))
                .ReverseMap();
            CreateMap<Tasks, TasksDto>()
                .ForMember(dest => dest.CreatedByName, src => src.MapFrom(a => a.CreatedUser == null ? "مدير النظام" : a.CreatedUser.FullName))
                .ReverseMap();
            CreateMap<PageResult<Tasks>, PageResult<TasksDto>>().ReverseMap();
            CreateMap<PageResult<Users>, PageResult<UsersDto>>().ReverseMap();
            CreateMap<AppUsers, AppUserDto>()
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(a => a.Company == null ? string.Empty : a.Company.NameAr))
                .ReverseMap();
            CreateMap<PageResult<AppUsers>, PageResult<AppUserDto>>().ReverseMap();
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<PageResult<BlackListApp>, PageResult<BlackListAppDto>>().ReverseMap();
            CreateMap<BlackListApp, BlackListAppDto>().ReverseMap();
            CreateMap<PageResult<DeviceBatteryTrans>, PageResult<DeviceBatteryTransDto>>().ReverseMap();
            CreateMap<DeviceBatteryTrans, DeviceBatteryTransDto>().ReverseMap();
            CreateMap<PageResult<DeviceUsage>, PageResult<DeviceUsageDto>>().ReverseMap();
            CreateMap<DeviceUsage, DeviceUsageDto>().ReverseMap();
            CreateMap<PageResult<CommandGroup>, PageResult<CommandGroupDto>>().ReverseMap();
            CreateMap<CommandGroup, CommandGroupDto>().ReverseMap();
            CreateMap<PageResult<DeviceLog>, PageResult<DeviceLogDto>>().ReverseMap();
            CreateMap<DeviceLog, DeviceLogDto>()
                                .ForMember(dest => dest.CreatedByName, src => src.MapFrom(a => a.CreatedUser == null ? "مدير النظام" : a.CreatedUser.FullName))
                .ReverseMap();
            CreateMap<PageResult<MobCentra.Domain.Entities.Profile>, PageResult<ProfileDto>>()
                .ReverseMap();
            CreateMap<MobCentra.Domain.Entities.Profile, ProfileDto>()
                 .ForMember(dest => dest.CreatedByName, src => src.MapFrom(a => a.User == null ? "مدير النظام" : a.User.FullName))
                 .ForMember(dest => dest.CompanyName, src => src.MapFrom(a => a.Company == null ? string.Empty : a.Company.NameAr))
                .ReverseMap();
            CreateMap<PageResult<Statistic>, PageResult<StatisticDto>>().ReverseMap();
            CreateMap<Statistic, StatisticDto>().ReverseMap();
            CreateMap<PageResult<DeviceQueu>, PageResult<DeviceQueuDto>>().ReverseMap();
            CreateMap<DeviceQueu, DeviceQueuDto>().ReverseMap();
            CreateMap<PageResult<DeviceStorageFile>, PageResult<DeviceStorageFileDto>>().ReverseMap();
            CreateMap<DeviceStorageFile, DeviceStorageFileDto>().ReverseMap();
            CreateMap<PageResult<DeviceFile>, PageResult<DeviceFileDto>>().ReverseMap();
            CreateMap<DeviceFile, DeviceFileDto>().ReverseMap();
            CreateMap<PageResult<Feature>, PageResult<FeatureDto>>().ReverseMap();
            CreateMap<Feature, FeatureDto>().ReverseMap();
            CreateMap<PageResult<DeviceTransaction>, PageResult<DeviceTransactionDto>>().ReverseMap();
            CreateMap<DeviceTransaction, DeviceTransactionDto>().ReverseMap();
            CreateMap<PageResult<DeviceApplication>, PageResult<DeviceApplicationDto>>().ReverseMap();
            CreateMap<DeviceApplication, DeviceApplicationDto>().ReverseMap();
            CreateMap<PageResult<DeviceNotification>, PageResult<DeviceNotificationDto>>().ReverseMap();
            CreateMap<DeviceNotification, DeviceNotificationDto>()
                .ForMember(dest => dest.DeviceName, src => src.MapFrom(a => a.Device == null ? string.Empty : a.Device.Name))
                .ReverseMap();
            CreateMap<PageResult<Domain.Entities.Application>, PageResult<ApplicationDto>>().ReverseMap();
            CreateMap<Domain.Entities.Application, ApplicationDto>()
                                 .ForMember(dest => dest.CreatedByName, src => src.MapFrom(a => a.User == null ? "مدير النظام" : a.User.FullName))
                                 .ReverseMap();
            CreateMap<PageResult<UserCommand>, PageResult<UserCommandDto>>().ReverseMap();
            CreateMap<UserCommand, UserCommandDto>()
                     .ForMember(dest => dest.GoogleCommandName, src => src.MapFrom(a => a.GoogleCommand == null ? string.Empty : a.GoogleCommand.Name))
                    .ForMember(dest => dest.GoogleCommandNameEn, src => src.MapFrom(a => a.GoogleCommand == null ? string.Empty : a.GoogleCommand.NameEn))
                .ReverseMap();

            CreateMap<PageResult<UserGroup>, PageResult<UserGroupDto>>().ReverseMap();
            CreateMap<UserGroup, UserGroupDto>().ReverseMap();
            CreateMap<PageResult<CompanySubscription>, PageResult<CompanySubscriptionDto>>().ReverseMap();
            CreateMap<CompanySubscription, CompanySubscriptionDto>().ReverseMap();
            CreateMap<PageResult<GeoFenc>, PageResult<GeoFencDto>>().ReverseMap();
            CreateMap<GeoFenc, GeoFencDto>().ReverseMap();
            CreateMap<GeoFencSetting, GeoFencSettingDto>().ReverseMap();
            CreateMap<PageResult<GeoFencSetting>, PageResult<GeoFencSettingDto>>().ReverseMap();

            CreateMap<PageResult<Company>, PageResult<CompanyDto>>().ReverseMap();
            CreateMap<Device, DeviceDto>()
                    .ForMember(dest => dest.CompanyName, src => src.MapFrom(a => a.Company == null ? string.Empty : a.Company.NameAr))
                    .ForMember(dest => dest.GroupName, src => src.MapFrom(a => a.Group == null ? string.Empty : a.Group.NameAr))
                    .ForMember(dest => dest.GroupNameOt, src => src.MapFrom(a => a.Group == null ? string.Empty : a.Group.NameOt))
                    .ForMember(dest => dest.ProfileName, src => src.MapFrom(a => a.Profile == null ? string.Empty : a.Profile.NameAr))
                    .ForMember(dest => dest.ProfileNameOt, src => src.MapFrom(a => a.Profile == null ? string.Empty : a.Profile.NameOt))
                    .ForMember(dest => dest.CurrentLocation, src => src.MapFrom(a => a.CurrentLocation == null ? string.Empty : $"{a.CurrentLocation.X},{a.CurrentLocation.Y}"));

            CreateMap<DeviceDto, Device>()
                    .ForMember(dest => dest.CurrentLocation, src => src.MapFrom(a => a.CurrentLocation == null ? null : NewPoint(a.CurrentLocation)));


            CreateMap<PageResult<Device>, PageResult<DeviceDto>>().ReverseMap();
            CreateMap<GoogleCommand, GoogleCommandDto>().ReverseMap();
            CreateMap<PageResult<GoogleCommand>, PageResult<GoogleCommandDto>>().ReverseMap();
            CreateMap<Group, GroupDto>()
                                 .ForMember(dest => dest.CreatedByName, src => src.MapFrom(a => a.User == null ? "مدير النظام" : a.User.FullName))

            .ForMember(dest => dest.CompanyName, src => src.MapFrom(a => a.Company == null ? string.Empty : a.Company.NameOt))
            .ReverseMap();

            CreateMap<PageResult<Group>, PageResult<GroupDto>>().ReverseMap();
            CreateMap<TaskStatus, TaskStatusDto>().ReverseMap();
            CreateMap<PageResult<TaskStatus>, PageResult<TaskStatusDto>>().ReverseMap();
            CreateMap<Transaction, TransactionDto>()
               .ForMember(dest => dest.AppUserName, src => src.MapFrom(a => a.AppUser == null ? string.Empty : a.AppUser.FullName))
               .ForMember(dest => dest.CompanyName, src => src.MapFrom(a => a.Company == null ? string.Empty : a.Company.NameAr))
               .ReverseMap();
            CreateMap<PageResult<Transaction>, PageResult<TransactionDto>>().ReverseMap();
            CreateMap<Setting, SettingDto>().ReverseMap();
            CreateMap<PageResult<Setting>, PageResult<SettingDto>>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<PageResult<Role>, PageResult<RoleDto>>().ReverseMap();
            CreateMap<Privilege, PrivilegeDto>()
             .ForMember(dest => dest.ParentName, src => src.MapFrom(a => a.Parent == null ? string.Empty : a.Parent.PrivilegeName))
            .ReverseMap();
            CreateMap<PageResult<Privilege>, PageResult<PrivilegeDto>>().ReverseMap();
            CreateMap<UserRole, UserRoleDto>()
             .ForMember(dest => dest.UserName, src => src.MapFrom(a => a.User == null ? string.Empty : a.User.FullName))
             .ForMember(dest => dest.RoleName, src => src.MapFrom(a => a.Role == null ? string.Empty : a.Role.NameAr))
             .ReverseMap();
            CreateMap<PageResult<UserRole>, PageResult<UserRoleDto>>().ReverseMap();

            CreateMap<ProfileFeature, ProfileFeatureDto>()
            .ForMember(dest => dest.FeatureName, src => src.MapFrom(a => a.Feature == null ? string.Empty : a.Feature.NameAr))
            .ForMember(dest => dest.ProfileName, src => src.MapFrom(a => a.Profile == null ? string.Empty : a.Profile.NameAr))
            .ReverseMap();
            CreateMap<PageResult<ProfileFeature>, PageResult<ProfileFeatureDto>>().ReverseMap();

            CreateMap<PageResult<City>, PageResult<CityDto>>().ReverseMap();
            CreateMap<Domain.Entities.Version, VersionDto>().ReverseMap();
            CreateMap<PageResult<Domain.Entities.Version>, PageResult<VersionDto>>().ReverseMap();

            CreateMap<RolePrivilege, RolePrivilegeDto>()
          .ForMember(dest => dest.PrivilegeName, src => src.MapFrom(a => a.Privilege == null ? string.Empty : a.Privilege.PrivilegeName))
          .ForMember(dest => dest.RoleName, src => src.MapFrom(a => a.Role == null ? string.Empty : a.Role.NameAr))
          .ReverseMap();
            CreateMap<PageResult<RolePrivilege>, PageResult<RolePrivilegeDto>>()
                .ReverseMap();
            CreateMap<Report, ReportDto>().ReverseMap();
            CreateMap<PageResult<Report>, PageResult<ReportDto>>().ReverseMap();
            CreateMap<ReportParameter, ReportParameterDto>().ReverseMap();
            CreateMap<PageResult<ReportParameter>, PageResult<ReportParameterDto>>().ReverseMap();
            CreateMap<Notifications, NotificationDto>()
                .ForMember(dest => dest.CreatedByName, src => src.MapFrom(a => a.CreatedUser == null ? "مدير النظام" : a.CreatedUser.FullName))
                .ReverseMap();
            CreateMap<PageResult<Notifications>, PageResult<NotificationDto>>().ReverseMap();
        }

        private Point NewPoint(string location)
        {
            if(string.IsNullOrEmpty(location)  || location.Split(",").Length < 2) return new Point(0, 0);
            return new Point(Convert.ToDouble(location.Split(",")[0]), Convert.ToDouble(location.Split(",")[1]));
        }
        public TDestination Map<TSource, TDestination>(TSource source) => _mapper.Map<TSource, TDestination>(source);

        public void Map<TSource, TDestination>(TSource source, TDestination destination) => Map(source, destination);

        public TDestination Map<TDestination>(object source) => _mapper.Map<TDestination>(source);
    }
}
