using Microsoft.EntityFrameworkCore;
using Namerd.Domain;

namespace Namerd.Persistence.Context;

public class NamerdContext : DbContext
{
    public NamerdContext(DbContextOptions<NamerdContext> options) : base(options)
    {
        
    }
    
    public DbSet<NamerdBot> Bots { get; set; }
    public DbSet<Settings> Settings { get; set; }
    public DbSet<NominationPeriod> NominationPeriods { get; set; }
    public DbSet<Nomination> Nominations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NamerdBot>()
            .HasMany(n => n.NominationPeriods)
            .WithOne(n => n.bot)
            .HasForeignKey(n => n.botId);

        modelBuilder.Entity<NominationPeriod>()
            .HasMany(n => n.NominationDetails)
            .WithOne(n => n.NominationPeriod)
            .HasForeignKey(n => n.NominationPeriodId);
    }
}