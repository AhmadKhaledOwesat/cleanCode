namespace DcpTracker.Application.Dto
{
    public class TransactionDto : BaseDto<Guid>
    {
        public string Coordinations { get; set; }
        public DateTime TransDateTime { get; set; }
        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid? AppUserId { get; set; }
        public string AppUserName { get; set; }
    }
}
