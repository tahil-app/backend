namespace Tahil.Infrastructure.EntityConfigurations;

public class StudentAttendanceConfiguration : IEntityTypeConfiguration<StudentAttendance>
{
    public void Configure(EntityTypeBuilder<StudentAttendance> builder)
    {
        builder.ToTable("student_attendance");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.StudentId)
            .HasColumnName("student_id");

        builder.Property(p => p.SessionId)
            .HasColumnName("session_id");

        builder.Property(p => p.Note)
            .HasColumnName("note");

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .IsRequired();

        builder.Property(p => p.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(p => p.TenantId)
            .HasColumnName("tenant_id");


        builder.HasOne(r => r.Student)
            .WithMany(r => r.StudentAttendances)
            .HasForeignKey(r => r.StudentId);

        builder.HasOne(r => r.Session)
            .WithMany(r => r.StudentAttendances)
            .HasForeignKey(r => r.SessionId);
    }
}