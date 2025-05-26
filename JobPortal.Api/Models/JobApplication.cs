using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Api.Models
{
    public class JobApplication
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid JobId { get; set; }

        [Required]
        public Guid ApplicantId { get; set; }

        public string? CoverLetter { get; set; }

        public string Status { get; set; } = "Submitted";  // or Viewed, Shortlisted, Rejected

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("JobId")]
        public JobPosting Job { get; set; }

        [ForeignKey("ApplicantId")]
        public User Applicant { get; set; }
    }
}
