using System.ComponentModel.DataAnnotations;

namespace MobCentra.Application.Dto
{
    public class UserLoginRequestDto
    {
        [Required]
        [StringLength(256)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string CompanyCode { get; set; } = string.Empty;
    }
}
