namespace MobCentra.Application.Dto
{
    public class CompanySubscriptionDto : BaseDto<Guid>
    {
        public Guid? CompanyId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
    }
}
