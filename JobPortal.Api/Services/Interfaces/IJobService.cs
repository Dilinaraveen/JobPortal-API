using JobPortal.Api.DTOs;

namespace JobPortal.Api.Services.Interfaces
{
    public interface IJobService
    {
        Task<GetJobDto?> GetJobByIdAsync(Guid jobId);
        Task<List<GetJobDto>> GetAllJobsAsync(string? applicantId);
        Task<GetJobDto> CreateJobAsync(JobCreateDto dto, Guid employerId);
        Task<GetJobDto?> UpdateJobAsync(Guid jobId, Guid employerId, JobUpdateDto dto);
        Task<bool> DeleteJobAsync(Guid jobId, Guid employerId);
    }
}
