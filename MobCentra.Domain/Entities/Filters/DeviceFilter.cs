namespace MobCentra.Domain.Entities.Filters
{
    public class DeviceFilter : SearchParameters<Device>
    {
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? GroupId { get; set; }
        public int? StatusId { get; set; }
        public int? PinnedStatusId { get; set; }
        public DateTime?  FromDate { get; set; }
        public string? Percentage { get; set; }
        public string? VersionNo { get; set; }
        public int? IsWrongTime { get; set; }


    }
}
