using MobCentra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TaskStatus = MobCentra.Domain.Entities.TaskStatus;

namespace MobCentra.Infrastructure.EfContext
{ 
    public class StudioContext(DbContextOptions<StudioContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<DevicesGeoFenceLog> DevicesGeoFenceLogs { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<GeoFencSetting> GeoFencSettings { get; set; }
        public DbSet<DeviceUsage> DeviceUsages { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<DeviceQueu> DeviceQueu { get; set; }
        public DbSet<GeoFenc> GeoFencs { get; set; }
        public DbSet<DeviceStorageFile> DeviceStorageFile { get; set; }
        public DbSet<CommandGroup> CommandGroups { get; set; }
        public DbSet<DeviceFile> DeviceFile { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<DeviceTransaction> DeviceTransactions { get; set; }
        public DbSet<DeviceLog> DeviceLogs { get; set; }
        public DbSet<DeviceApplication> DeviceApplications { get; set; }
        public DbSet<DeviceNotification> DeviceNotifications { get; set; }
        public DbSet<Domain.Entities.Application> Applications { get; set; }
        public DbSet<CompanySubscription> CompanySubscriptions { get; set; }
        public DbSet<BlackListApp> BlackListApps { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<MDMCommand> MDMCommands { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<AppUsers> AppUser { get; set; }
        public DbSet<Company> Countries { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Privilege> Privilege { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserCommand> UserCommands { get; set; }
        public DbSet<Domain.Entities.Version> Versions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RolePrivilege> RolePrivilege { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportParameter> ReportParameter { get; set; }
        public DbSet<TaskStatus> TaskStatus { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileFeature> ProfileFeatures { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<DeviceBatteryTrans> DeviceBatteryTrans { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<DeviceNotification>().ToTable("DeviceNotifications");
            modelBuilder.Entity<City>().ToTable("City").Property(a => a.Area).HasColumnType("geometry");
            modelBuilder.Entity<DeviceQueu>().ToTable("DeviceQueus");
            modelBuilder.Entity<Device>().ToTable("Devices").Property(a => a.CurrentLocation).HasColumnType("geometry");
            modelBuilder.Entity<GeoFenc>().ToTable("GeoFencs");
            modelBuilder.Entity<DevicesGeoFenceLog>().ToTable("DevicesGeoFenceLog").Property(a => a.Coordinations).HasColumnType("geometry");

        }
    }
}
