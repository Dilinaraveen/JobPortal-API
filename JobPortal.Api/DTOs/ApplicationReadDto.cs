using System;

namespace JobPortal.Api.DTOs
{
    public class ApplicationReadDto
    {
        public Guid Id { get; set; }

        public string JobTitle { get; set; }

        public string? CoverLetter { get; set; }

        public string Status { get; set; }

        public DateTime AppliedAt { get; set; }

        public string ApplicantName { get; set; }

        public string ApplicantEmail { get; set; }
    }
}
