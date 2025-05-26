// JobService.cs (based on controller logic)
using Microsoft.EntityFrameworkCore;
using JobPortal.Api.Data;
using JobPortal.Api.DTOs;
using JobPortal.Api.Models;
using JobPortal.Api.Services.Interfaces;

namespace JobPortal.Api.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;

        public JobService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetJobDto> CreateJobAsync(JobCreateDto dto, Guid employerId)
        {
            var job = new JobPosting
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                JobType = dto.JobType,
                SalaryMin = dto.SalaryMin,
                SalaryMax = dto.SalaryMax,
                CreatedAt = DateTime.UtcNow,
                EmployerId = employerId,
                IsActive = true
            };

            _context.JobPostings.Add(job);
            await _context.SaveChangesAsync();

            return new GetJobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                JobType = job.JobType,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                CreatedAt = job.CreatedAt,
                EmployerName = "",
                EmployerEmail = ""
            };
        }

        public async Task<List<GetJobDto>> GetAllJobsAsync(string? applicantId)
        {
            var jobs = await _context.JobPostings
                .Include(j => j.Employer)
                .Where(j => j.IsActive)
                .ToListAsync();

            var results = new List<GetJobDto>();

            foreach (var job in jobs)
            {
                var status = applicantId != null
                    ? await _context.JobApplications
                        .Where(a => a.JobId == job.Id && a.ApplicantId == Guid.Parse(applicantId))
                        .Select(a => a.Status)
                        .FirstOrDefaultAsync()
                    : null;

                results.Add(new GetJobDto
                {
                    Id = job.Id,
                    Title = job.Title,
                    Description = job.Description,
                    Location = job.Location,
                    JobType = job.JobType,
                    SalaryMin = job.SalaryMin,
                    SalaryMax = job.SalaryMax,
                    CreatedAt = job.CreatedAt,
                    EmployerName = job.Employer.FullName,
                    EmployerEmail = job.Employer.Email,
                });
            }

            return results;
        }
        public async Task<GetJobDto?> GetJobByIdAsync(Guid id)
        {
            var job = await _context.JobPostings
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(j => j.Id == id && j.IsActive);

            if (job == null) return null;

            return new GetJobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                JobType = job.JobType,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                CreatedAt = job.CreatedAt,
                EmployerName = job.Employer.FullName,
                EmployerEmail = job.Employer.Email
            };
        }

        public async Task<GetJobDto?> UpdateJobAsync(Guid id, Guid employerId, JobUpdateDto dto)
        {
            var job = await _context.JobPostings.Include(j => j.Employer).FirstOrDefaultAsync(j => j.Id == id);
            if (job == null || job.EmployerId != employerId)
                return null;

            job.Title = dto.Title;
            job.Description = dto.Description;
            job.Location = dto.Location;
            job.JobType = dto.JobType;
            job.SalaryMin = dto.SalaryMin;
            job.SalaryMax = dto.SalaryMax;

            await _context.SaveChangesAsync();

            return new GetJobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                JobType = job.JobType,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                CreatedAt = job.CreatedAt,
                EmployerName = job.Employer.FullName,
                EmployerEmail = job.Employer.Email
            };
        }

        public async Task<bool> DeleteJobAsync(Guid id, Guid employerId)
        {
            var job = await _context.JobPostings.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null || job.EmployerId != employerId)
                return false;

            job.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
