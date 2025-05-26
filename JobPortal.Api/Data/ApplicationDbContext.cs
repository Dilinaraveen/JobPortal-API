using Microsoft.EntityFrameworkCore;
using JobPortal.Api.Models;

namespace JobPortal.Api.Data
{
    public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<SavedJob> SavedJobs { get; set; }
}

}
