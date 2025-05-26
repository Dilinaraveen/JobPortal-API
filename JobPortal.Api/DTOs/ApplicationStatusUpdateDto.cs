using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class ApplicationStatusUpdateDto
    {
        [Required]
        public string Status { get; set; }
    }
}
