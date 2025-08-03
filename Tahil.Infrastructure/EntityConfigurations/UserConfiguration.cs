namespace Tahil.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(p => p.PhoneNumber)
            .HasColumnName("phone_number");

        builder.Property(p => p.Role)
            .HasColumnName("role")
            .IsRequired();

        builder.Property(p => p.Gender)
            .HasColumnName("gender");

        builder.Property(p => p.JoinedDate)
            .HasColumnName("joined_date");

        builder.Property(p => p.BirthDate)
            .HasColumnName("birth_date");

        builder.Property(p => p.ImagePath)
            .HasColumnName("image_path");

        builder.Property(p => p.TenantId)
            .HasColumnName("tenant_id");

        builder.OwnsOne(p => p.Email, emailBuilder =>
        {
            emailBuilder.Property(p => p.Value)
                .HasColumnName("email")
                .IsRequired();
        });

        builder.Property(p => p.Password)
            .HasColumnName("password")
            .IsRequired();

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active");

        builder.HasOne(r => r.Tenant)
            .WithMany(r => r.Users)
            .HasForeignKey(r => r.TenantId);
    }
}