
namespace DcpTracker.Application.Dto
{
    public class ReportDto : BaseDto<Guid>
    {
        public string ReportName { get; set; }
        public string ReportProcedure { get; set; }
        public Guid? CompanyId { get; set; }
        public ICollection<ReportParameterDto> ReportParameters { get; set; } = [];
    }
}
