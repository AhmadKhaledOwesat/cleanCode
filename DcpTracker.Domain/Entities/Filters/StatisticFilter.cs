namespace MobCentra.Domain.Entities.Filters
{
    public class StatisticFilter : SearchParameters<Statistic>
    {
        public int? Active { get; set; }
        public Guid? CompanyId { get; set; }    

    }
}
