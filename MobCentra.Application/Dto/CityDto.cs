namespace MobCentra.Application.Dto
{
    public class CityDto : BaseDto<Guid>
    {
        public string Name { get; set; }
        public string NameOt { get; set; }
        public string Location { get; set; }
        public Guid? CompanyId { get; set; }
        public string CreatedByName { get; set; }
    }
}
