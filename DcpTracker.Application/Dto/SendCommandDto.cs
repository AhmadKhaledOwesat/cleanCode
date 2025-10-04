namespace MobCentra.Application.Dto
{
    public class SendCommandDto 
    {
        public string Command { get; set; }
        public string[] Token { get; set; }
        public Guid? GroupId { get; set; }
        public string ApkUrl { get; set; }
        public  string Password { get; set; }
        public string PackageName { get; set; }
        public string WallpaperUrl { get; set; }
        public Guid? CompanyId { get; set; }
        public bool IgnoreExecption { get; set; }
        public bool IsInternal { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
    public class SendNotifyDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string[] Token { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? CompanyId { get; set; } 
    }
}
