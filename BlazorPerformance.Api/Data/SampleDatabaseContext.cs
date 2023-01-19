using Microsoft.EntityFrameworkCore;
using BlazorPerformance.Shared.Models;

namespace BlazorPerformance.Api.Data;

public class SampleDatabaseContext : DbContext
{
    public SampleDatabaseContext(DbContextOptions<SampleDatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<Conference> Conferences { get; set; }
    public DbSet<Contribution> Contributions { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<ContributionSpeakerMapping> ContributionSpeakerMappings { get; set; }
}
