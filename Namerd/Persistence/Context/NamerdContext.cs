using Microsoft.EntityFrameworkCore;
using Namerd.Domain.Entities;

namespace Namerd.Persistence.Context;

public class NamerdContext : DbContext
{
    public NamerdContext(DbContextOptions<NamerdContext> options) : base(options)
    {
        
    }
    
    public DbSet<NamerdBot> Bots { get; set; }
    public DbSet<Settings> Settings { get; set; }
    public DbSet<MonthlyNomination> MonthlyNominations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NamerdBot>()
            .HasMany(n => n.MonthlyNomination)
            .WithOne(n => n.bot)
            .HasForeignKey(n => n.botId);
    }
}