using System.ComponentModel.DataAnnotations;

namespace DMS.Domain.Dto.Authentication
{
    public class AuthRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}