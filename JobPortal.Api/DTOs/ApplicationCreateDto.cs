using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class ApplicationCreateDto
    {
        [Required]
        public Guid JobId { get; set; }

        public string? CoverLetter { get; set; }
    }
}
