using System.ComponentModel.DataAnnotations;

namespace MobCentra.Application.Dto
{
    public class ResetPasswordRequestDto
    {
        [Required]
        [StringLength(256)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string CompanyCode { get; set; } = string.Empty;
    }
}
