// JobController (refactored version using Services & Repositories)
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobPortal.Api.DTOs;
using JobPortal.Api.Services.Interfaces;

namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employer")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob(JobCreateDto dto)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null) return Unauthorized();

            var result = await _jobService.CreateJobAsync(dto, Guid.Parse(employerId));
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobs = await _jobService.GetAllJobsAsync(userId);
            return Ok(jobs);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(Guid id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound("Job not found.");
            return Ok(job);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, JobUpdateDto dto)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null) return Unauthorized();

            var result = await _jobService.UpdateJobAsync(id, Guid.Parse(employerId), dto);
            return result == null ? Forbid("You are not authorized to update this job.") : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null) return Unauthorized();

            var success = await _jobService.DeleteJobAsync(id, Guid.Parse(employerId));
            return success ? Ok("Job marked as inactive.") : Forbid("You are not authorized to delete this job.");
        }
    }
}
