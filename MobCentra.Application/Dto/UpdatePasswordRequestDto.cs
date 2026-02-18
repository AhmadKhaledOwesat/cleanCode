using System.ComponentModel.DataAnnotations;

namespace MobCentra.Application.Dto
{
    public class UpdatePasswordRequestDto
    {
        public Guid UserId { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
