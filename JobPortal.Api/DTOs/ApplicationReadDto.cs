using System;

namespace JobPortal.Api.DTOs
{
    public class ApplicationReadDto
    {
        public Guid Id { get; set; }

        public required string JobTitle { get; set; }

        public string? CoverLetter { get; set; }

        public required string Status { get; set; }

        public DateTime AppliedAt { get; set; }

        public required string ApplicantName { get; set; }

        public required string ApplicantEmail { get; set; }
    }
}
