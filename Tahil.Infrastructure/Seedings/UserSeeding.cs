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
                TenantId = new Guid("6AF39530-F6E0-4298-A890-FB5C50310C7C"),
                Password = "$2a$11$GGEGcXpeP39kYGG6NFIS3O0EskcYWfucTDcK8Y8kBLJw93iDqcjSG" //PasswordHasher.Hash("admin")
            }
        );

        modelBuilder.Entity<User>().OwnsOne(u => u.Email).HasData(
            new { UserId = 1, Value = "admin@be.com" }
        );
    }
}