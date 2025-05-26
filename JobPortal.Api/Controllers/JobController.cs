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
                .ToListAsync();

            return Ok(jobs);
        }
    }
}
