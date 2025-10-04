using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Application.Dto
{
    public class AppUserDto : BaseDto<Guid>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public int Active { get; set; }
        public string DeviceId { get; set; }
        public string GpsLocation { get; set; }
        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string PushNotificationToken { get; set; }
        [NotMapped]
        public List<SettingDto> Settings { get; set; }
    }
}
