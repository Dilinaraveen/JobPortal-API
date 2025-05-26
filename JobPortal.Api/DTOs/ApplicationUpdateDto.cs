using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class ApplicationUpdateDto
    {
        [Required]
        public required string CoverLetter { get; set; }
    }
}
