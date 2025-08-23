namespace Tahil.Infrastructure.Seedings;

public static class UserSeeding
{
    public static void SeedUsers(this ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                IsActive = true, 
                PhoneNumber = "0000",
                Name = "Admin",
                Role = Domain.Enums.UserRole.Admin,
                TenantId = Tenants.DarAlfor2an,
                Password = "$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K" //PasswordHasher.Hash("admin")
            }
        );

        modelBuilder.Entity<User>().OwnsOne(u => u.Email).HasData(
            new { UserId = 1, Value = "admin@be.com" }
        );
    }
}