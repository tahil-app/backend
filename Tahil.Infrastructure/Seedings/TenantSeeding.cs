namespace Tahil.Infrastructure.Seedings;

public static class TenantSeeding
{
    public static void SeedTenants(this ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<Tenant>().HasData(
            new Tenant
            { 
                Id = new Guid("6AF39530-F6E0-4298-A890-FB5C50310C7C"),
                Name = "دار الفرقان",
                IsActive = true
            }
        );

    }
}