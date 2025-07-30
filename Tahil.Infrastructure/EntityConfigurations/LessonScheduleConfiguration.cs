namespace Tahil.Infrastructure.EntityConfigurations;

public class LessonScheduleConfiguration : IEntityTypeConfiguration<LessonSchedule>
{
    public void Configure(EntityTypeBuilder<LessonSchedule> builder)
    {
        builder.ToTable("lesson_schedule");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.RoomId)
            .HasColumnName("room_id")
            .IsRequired();

        builder.Property(p => p.CourseId)
            .HasColumnName("course_id")
            .IsRequired();

        builder.Property(p => p.TeacherId)
            .HasColumnName("teacher_id")
            .IsRequired();

        builder.Property(p => p.GroupId)
            .HasColumnName("group_id")
            .IsRequired();

        builder.Property(p => p.Day)
            .HasColumnName("day")
            .IsRequired();

        builder.Property(p => p.StartTime)
            .HasColumnName("start_time");

        builder.Property(p => p.EndTime)
            .HasColumnName("end_time");

        builder.Property(p => p.StartDate)
            .HasColumnName("start_date");

        builder.Property(p => p.EndDate)
            .HasColumnName("end_date");

        builder.Property(p => p.Status)
            .HasColumnName("status");

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

        // Navigation properties
        builder.HasOne(p => p.Room)
            .WithMany(r => r.Schedules)
            .HasForeignKey(p => p.RoomId);

        builder.HasOne(p => p.Course)
            .WithMany(r => r.Schedules)
            .HasForeignKey(p => p.CourseId);

        builder.HasOne(p => p.Teacher)
            .WithMany(r => r.Schedules)
            .HasForeignKey(p => p.TeacherId);

        builder.HasOne(p => p.Group)
            .WithMany(r => r.Schedules)
            .HasForeignKey(p => p.GroupId);
    }
}