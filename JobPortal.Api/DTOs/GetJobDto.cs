using System;

namespace JobPortal.Api.DTOs
{
    public class GetJobDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }
        public required string JobType { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string EmployerName { get; set; }
        public required string EmployerEmail { get; set; }
    }
}
