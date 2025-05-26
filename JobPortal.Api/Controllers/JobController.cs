using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using JobPortal.Api.Data;
using JobPortal.Api.DTOs;
using JobPortal.Api.Models;

namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employer")] // ✅ Only logged-in employers
    public class JobController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob(JobCreateDto dto)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? User.FindFirstValue(ClaimTypes.Name); // fallback

            if (employerId == null)
                return Unauthorized();

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
                EmployerId = Guid.Parse(employerId)
            };

            _context.JobPostings.Add(job);
            await _context.SaveChangesAsync();

            return Ok(job);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _context.JobPostings
                .Include(j => j.Employer)
                .Where(j => j.IsActive)
                .Select(job => new GetJobDto
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
                })
                .ToListAsync();

            return Ok(jobs);
        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(Guid id)
        {
            var job = await _context.JobPostings
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(j => j.Id == id && j.IsActive);

            if (job == null)
                return NotFound("Job not found.");

            var dto = new GetJobDto
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

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, JobUpdateDto dto)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null)
                return Unauthorized();

            var job = await _context.JobPostings.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null || job.EmployerId != Guid.Parse(employerId))
                return Forbid("You are not authorized to update this job.");

            job.Title = dto.Title;
            job.Description = dto.Description;
            job.Location = dto.Location;
            job.JobType = dto.JobType;
            job.SalaryMin = dto.SalaryMin;
            job.SalaryMax = dto.SalaryMax;

            await _context.SaveChangesAsync();
            return Ok(job);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null)
                return Unauthorized();

            var job = await _context.JobPostings.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null || job.EmployerId != Guid.Parse(employerId))
                return Forbid("You are not authorized to delete this job.");

            job.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok("Job marked as inactive.");
        }

    }
}
