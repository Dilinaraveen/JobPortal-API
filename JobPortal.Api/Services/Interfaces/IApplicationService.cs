// IApplicationService.cs
using JobPortal.Api.DTOs;

namespace JobPortal.Api.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<string?> ApplyAsync(ApplicationCreateDto dto, Guid applicantId);
        Task<List<ApplicationReadDto>> GetMyApplicationsAsync(Guid applicantId);
        Task<List<ApplicationReadDto>?> GetApplicationsForJobAsync(Guid jobId, Guid employerId);
        Task<string?> UpdateApplicationStatusAsync(Guid applicationId, Guid employerId, string newStatus);
        Task<string?> WithdrawApplicationAsync(Guid applicationId, Guid applicantId);
        Task<string?> UpdateApplicationAsync(Guid applicationId, Guid applicantId, string coverLetter);
    }
}
