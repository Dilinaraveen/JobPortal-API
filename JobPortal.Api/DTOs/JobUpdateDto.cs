using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class JobUpdateDto
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string Location { get; set; }

        [Required]
        public required string JobType { get; set; }

        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
    }
}
