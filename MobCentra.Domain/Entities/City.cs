using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
{
    public class City : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string NameOt { get; set; }
        public string Location { get; set; }
        public Guid? CompanyId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual Users User { get; set; }
    }
}
