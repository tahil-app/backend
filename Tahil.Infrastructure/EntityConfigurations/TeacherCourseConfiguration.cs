namespace Tahil.Infrastructure.EntityConfigurations;

public class TeacherCourseConfiguration : IEntityTypeConfiguration<TeacherCourse>
{
    public void Configure(EntityTypeBuilder<TeacherCourse> builder)
    {
        builder.ToTable("teacher_course");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.TeacherId)
            .HasColumnName("teacher_id");

        builder.Property(p => p.CourseId)
            .HasColumnName("course_id");

        builder.HasOne(r => r.Teacher)
            .WithMany(r => r.TeacherCourses)
            .HasForeignKey(r => r.TeacherId);

        builder.HasOne(r => r.Course)
            .WithMany(r => r.TeacherCourses)
            .HasForeignKey(r => r.CourseId);
    }
}