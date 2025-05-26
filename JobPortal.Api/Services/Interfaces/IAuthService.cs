// IAuthService.cs
using JobPortal.Api.DTOs;

namespace JobPortal.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string? Token, string? Error)> RegisterAsync(UserRegisterDto dto);
        Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginDto dto);
    }
}
