using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        public required string Role { get; set; }
    }
}
