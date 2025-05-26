using JobPortal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Api.Data;

public static class DbInitializer
{
    public static void Seed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Auto-migrate
        context.Database.Migrate();

        // Seed Users
        if (!context.Users.Any())
        {
            var users = new List<User>
                {
                    new() {
                        Id = Guid.NewGuid(),
                        FullName = "Employer One",
                        Email = "employer1@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                        Role = "Employer",
                        CreatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        FullName = "Applicant One",
                        Email = "applicant1@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                        Role = "Applicant",
                        CreatedAt = DateTime.UtcNow
                    }
                };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // Seed Jobs (if any employers exist)
        var employer = context.Users.FirstOrDefault(u => u.Role == "Employer");
        if (employer != null && !context.JobPostings.Any())
        {
            context.JobPostings.Add(new JobPosting
            {
                Id = Guid.NewGuid(),
                Title = "Junior .NET Developer",
                Description = "Join our growing software team!",
                Location = "Colombo",
                JobType = "Full-Time",
                SalaryMin = 80000,
                SalaryMax = 120000,
                CreatedAt = DateTime.UtcNow,
                EmployerId = employer.Id
            });

            context.SaveChanges();
        }
    }
}

