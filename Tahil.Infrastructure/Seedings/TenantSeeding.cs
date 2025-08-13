namespace Tahil.Infrastructure.Seedings;

public static class TenantSeeding
{
    public static void SeedTenants(this ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<Tenant>().HasData(
            new Tenant
            { 
                Id = Tenants.DarAlfor2an,
                Name = "دار الفرقان",
                IsActive = true
            }
        );

    }
}