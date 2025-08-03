namespace Tahil.Infrastructure.EntityConfigurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("course");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description");

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active");

        builder.Property(p => p.TenantId)
            .HasColumnName("tenant_id");

        builder.HasOne(r => r.Tenant)
            .WithMany(r => r.Courses)
            .HasForeignKey(r => r.TenantId);
    }
}