using System;

namespace JobPortal.Api.DTOs
{
    public class GetJobDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EmployerName { get; set; }
        public string EmployerEmail { get; set; }
    }
}
