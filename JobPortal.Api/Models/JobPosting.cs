using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Api.Models
{
    public class JobPosting
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string JobType { get; set; }  // e.g., Full-Time, Part-Time

        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;

        // FK + Navigation
        [Required]
        public Guid EmployerId { get; set; }

        [ForeignKey("EmployerId")]
        public User Employer { get; set; }

        public ICollection<JobApplication>? Applications { get; set; }
        public ICollection<SavedJob>? SavedByUsers { get; set; }
    }
}
