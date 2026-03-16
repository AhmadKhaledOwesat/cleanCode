using System.ComponentModel.DataAnnotations.Schema;

namespace MobCentra.Domain.Entities
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

    public class ProfileApplication : BaseEntity<Guid>
    {
        public Guid ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }

        public Guid ApplicationsId { get; set; }
        [ForeignKey(nameof(ApplicationsId))]
        public virtual Application Application { get; set; }
    }
}
