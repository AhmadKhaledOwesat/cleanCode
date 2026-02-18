namespace MobCentra.Application.Dto
{
    public class GroupDto : BaseDto<Guid>
    {
        public string NameAr { get; set; }
        public string NameOt { get; set; }
        public int Active { get; set; }
        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CreatedByName { get; set; }


    }
}
