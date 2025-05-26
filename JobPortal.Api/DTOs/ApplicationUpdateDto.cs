using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class ApplicationUpdateDto
    {
        [Required]
        public string CoverLetter { get; set; }
    }
}
