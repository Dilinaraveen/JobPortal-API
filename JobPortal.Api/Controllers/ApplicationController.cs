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
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApplicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ 1. Apply to a job
        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplicationCreateDto dto)
        {
            var applicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (applicantId == null)
                return Unauthorized();

            // Prevent duplicate application
            var alreadyApplied = await _context.JobApplications.AnyAsync(a =>
                a.JobId == dto.JobId && a.ApplicantId == Guid.Parse(applicantId));
            if (alreadyApplied)
                return Conflict("You have already applied to this job.");

            var application = new JobApplication
            {
                Id = Guid.NewGuid(),
                JobId = dto.JobId,
                ApplicantId = Guid.Parse(applicantId),
                CoverLetter = dto.CoverLetter
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            return Ok("Application submitted successfully.");
        }

        // ✅ 2. Get your own applications
        [Authorize(Roles = "Applicant")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyApplications()
        {
            var applicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (applicantId == null)
                return Unauthorized();

            var applications = await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.ApplicantId == Guid.Parse(applicantId))
                .Select(a => new ApplicationReadDto
                {
                    Id = a.Id,
                    JobTitle = a.Job.Title,
                    CoverLetter = a.CoverLetter,
                    Status = a.Status,
                    AppliedAt = a.AppliedAt,
                    ApplicantName = a.Applicant.FullName,
                    ApplicantEmail = a.Applicant.Email
                })
                .ToListAsync();

            return Ok(applications);
        }

        // ✅ 3. Get applications for a job (employer only)
        [Authorize(Roles = "Employer")]
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetApplicationsForJob(Guid jobId)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null)
                return Unauthorized("You are not allowed to view applications for this job.");

            var job = await _context.JobPostings
                .FirstOrDefaultAsync(j => j.Id == jobId && j.EmployerId == Guid.Parse(employerId));

            if (job == null)
                return Unauthorized("You are not allowed to view applications for this job.");

            var applications = await _context.JobApplications
                .Include(a => a.Applicant)
                .Where(a => a.JobId == jobId)
                .Select(a => new ApplicationReadDto
                {
                    Id = a.Id,
                    JobTitle = job.Title,
                    CoverLetter = a.CoverLetter,
                    Status = a.Status,
                    AppliedAt = a.AppliedAt,
                    ApplicantName = a.Applicant.FullName,
                    ApplicantEmail = a.Applicant.Email
                })
                .ToListAsync();

            return Ok(applications);
        }

        // ✅ Update status of an application (Employer only)
        [Authorize(Roles = "Employer")]
        [HttpPatch("{applicationId}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(Guid applicationId, ApplicationStatusUpdateDto dto)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null)
                return Unauthorized();

            var application = await _context.JobApplications
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return NotFound("Application not found.");

            if (application.Job.EmployerId != Guid.Parse(employerId))
                return StatusCode(403, "You are not authorized to update this application.");

            application.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok("Application status updated successfully.");
        }

    }
}
