namespace Tahil.Infrastructure.EntityConfigurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("group");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(p => p.CourseId)
            .HasColumnName("course_id");

        builder.HasOne(r => r.Course)
            .WithMany(r => r.Groups)
            .HasForeignKey(r => r.CourseId);

        builder.Property(p => p.TeacherId)
            .HasColumnName("teacher_id");

        builder.HasOne(r => r.Teacher)
            .WithMany(r => r.Groups)
            .HasForeignKey(r => r.TeacherId);

        builder.Property(p => p.TenantId)
            .HasColumnName("tenant_id");

        builder.HasOne(r => r.Tenant)
            .WithMany(r => r.Groups)
            .HasForeignKey(r => r.TenantId);
    }
}