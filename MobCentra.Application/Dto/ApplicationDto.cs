namespace MobCentra.Application.Dto
{
    public class ApplicationDto : BaseDto<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public string File { get; set; }
        public Guid? CompanyId { get; set; }
        public string PackageName { get; set; }
        public string Label { get; set; }

        public string VersionName { get; set; }
        public long? AppSize { get; set; }
        public string CreatedByName { get; set; }

    }
}
