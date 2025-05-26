using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class JobUpdateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string JobType { get; set; }

        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
    }
}
