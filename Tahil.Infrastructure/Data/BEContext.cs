using Tahil.Infrastructure.Seedings;
using System.Reflection;

namespace Tahil.Infrastructure.Data;

public class BEContext : DbContext
{
    public BEContext(DbContextOptions options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.SeedUsers();
        base.OnModelCreating(modelBuilder);
    }
}
