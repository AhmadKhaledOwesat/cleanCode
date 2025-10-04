using System.ComponentModel.DataAnnotations.Schema;

namespace DcpTracker.Domain.Entities
{
    public class ProfileFeature : BaseEntity<Guid>
    {
        public Guid ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }

        public int FeatureId { get; set; }
        [ForeignKey(nameof(FeatureId))]
        public virtual Feature Feature { get; set; }  
    }
}
