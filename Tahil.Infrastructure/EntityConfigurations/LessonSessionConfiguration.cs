namespace Tahil.Infrastructure.EntityConfigurations;

public class LessonSessionConfiguration : IEntityTypeConfiguration<LessonSession>
{
    public void Configure(EntityTypeBuilder<LessonSession> builder)
    {
        builder.ToTable("lesson_session");

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

        builder.Property(p => p.ScheduleId)
            .HasColumnName("schedule_id")
            .IsRequired();

        builder.Property(p => p.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(p => p.Note)
            .HasColumnName("note");

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
            .WithMany(p => p.Sessions)
            .HasForeignKey(p => p.RoomId);

        builder.HasOne(p => p.Course)
            .WithMany(p => p.Sessions)
            .HasForeignKey(p => p.CourseId);

        builder.HasOne(p => p.Teacher)
            .WithMany(p => p.Sessions)
            .HasForeignKey(p => p.TeacherId);

        builder.HasOne(p => p.Group)
            .WithMany(p => p.Sessions)
            .HasForeignKey(p => p.GroupId);

        builder.HasOne(p => p.Schedule)
            .WithMany(s => s.Sessions)
            .HasForeignKey(p => p.ScheduleId);
    }
}