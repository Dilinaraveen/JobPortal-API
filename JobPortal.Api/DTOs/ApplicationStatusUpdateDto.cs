using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class ApplicationStatusUpdateDto
    {
        [Required]
        public required string Status { get; set; }
    }
}
