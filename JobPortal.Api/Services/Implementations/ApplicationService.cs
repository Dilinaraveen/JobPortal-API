// ApplicationService.cs
using JobPortal.Api.Data;
using JobPortal.Api.DTOs;
using JobPortal.Api.Models;
using JobPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Api.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ApplyAsync(ApplicationCreateDto dto, Guid applicantId)
        {
            bool alreadyApplied = await _context.JobApplications.AnyAsync(a =>
                a.JobId == dto.JobId && a.ApplicantId == applicantId);

            if (alreadyApplied)
                return "You have already applied to this job.";

            var application = new JobApplication
            {
                Id = Guid.NewGuid(),
                JobId = dto.JobId,
                ApplicantId = applicantId,
                CoverLetter = dto.CoverLetter
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<List<ApplicationReadDto>> GetMyApplicationsAsync(Guid applicantId)
        {
            return await _context.JobApplications
                .Include(a => a.Job)
                .Include(a => a.Applicant)
                .Where(a => a.ApplicantId == applicantId)
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
        }

        public async Task<List<ApplicationReadDto>?> GetApplicationsForJobAsync(Guid jobId, Guid employerId)
        {
            var job = await _context.JobPostings.FirstOrDefaultAsync(j => j.Id == jobId && j.EmployerId == employerId);
            if (job == null) return null;

            return await _context.JobApplications
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
        }

        public async Task<string?> UpdateApplicationStatusAsync(Guid applicationId, Guid employerId, string newStatus)
        {
            var application = await _context.JobApplications
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return "Application not found.";

            if (application.Job.EmployerId != employerId)
                return "You are not authorized to update this application.";

            application.Status = newStatus;
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string?> WithdrawApplicationAsync(Guid applicationId, Guid applicantId)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.ApplicantId == applicantId);

            if (application == null)
                return "Application not found or access denied.";

            if (application.Status != "Submitted")
                return $"Cannot withdraw application after it has been {application.Status.ToLower()}.";

            _context.JobApplications.Remove(application);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string?> UpdateApplicationAsync(Guid applicationId, Guid applicantId, string coverLetter)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.ApplicantId == applicantId);

            if (application == null)
                return "Application not found or access denied.";

            if (application.Status != "Submitted")
                return $"Cannot update application after it has been {application.Status.ToLower()}.";

            application.CoverLetter = coverLetter;
            await _context.SaveChangesAsync();

            return null;
        }
    }
}
