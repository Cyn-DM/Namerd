using Microsoft.EntityFrameworkCore;
using Namerd.Domain;

namespace Namerd.Persistence.Context;

public class NamerdContext : DbContext
{
    public NamerdContext(DbContextOptions<NamerdContext> options) : base(options)
    {
        
    }
    
    public DbSet<NamerdBot> Bots { get; set; }
}