// ApplicationController.cs (Refactored)
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobPortal.Api.DTOs;
using JobPortal.Api.Services.Interfaces;

namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplicationCreateDto dto)
        {
            var applicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (applicantId == null) return Unauthorized();

            var result = await _applicationService.ApplyAsync(dto, Guid.Parse(applicantId));
            return result == null ? Ok("Application submitted successfully.") : Conflict(result);
        }

        [Authorize(Roles = "Applicant")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyApplications()
        {
            var applicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (applicantId == null) return Unauthorized();

            var applications = await _applicationService.GetMyApplicationsAsync(Guid.Parse(applicantId));
            return Ok(applications);
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetApplicationsForJob(Guid jobId)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null) return Unauthorized();

            var apps = await _applicationService.GetApplicationsForJobAsync(jobId, Guid.Parse(employerId));
            return apps == null ? Unauthorized("You are not allowed to view applications for this job.") : Ok(apps);
        }

        [Authorize(Roles = "Employer")]
        [HttpPatch("{applicationId}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(Guid applicationId, ApplicationStatusUpdateDto dto)
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employerId == null) return Unauthorized();

            var result = await _applicationService.UpdateApplicationStatusAsync(applicationId, Guid.Parse(employerId), dto.Status);
            return result == null ? Ok("Application status updated successfully.") : StatusCode(403, result);
        }

        [Authorize(Roles = "Applicant")]
        [HttpDelete("{applicationId}")]
        public async Task<IActionResult> WithdrawApplication(Guid applicationId)
        {
            var applicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (applicantId == null) return Unauthorized();

            var result = await _applicationService.WithdrawApplicationAsync(applicationId, Guid.Parse(applicantId));
            return result == null ? Ok("Application withdrawn successfully.") : StatusCode(403, result);
        }

        [Authorize(Roles = "Applicant")]
        [HttpPut("{applicationId}")]
        public async Task<IActionResult> UpdateApplication(Guid applicationId, ApplicationUpdateDto dto)
        {
            var applicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (applicantId == null) return Unauthorized();

            var result = await _applicationService.UpdateApplicationAsync(applicationId, Guid.Parse(applicantId), dto.CoverLetter);
            return result == null ? Ok("Application updated successfully.") : StatusCode(403, result);
        }
    }
}
