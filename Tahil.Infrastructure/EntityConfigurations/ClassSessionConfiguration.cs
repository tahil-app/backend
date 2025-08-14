namespace Tahil.Infrastructure.EntityConfigurations;

public class ClassSessionConfiguration : IEntityTypeConfiguration<ClassSession>
{
    public void Configure(EntityTypeBuilder<ClassSession> builder)
    {
        builder.ToTable("class_session");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnName("status");

        builder.Property(p => p.ScheduleId)
            .HasColumnName("schedule_id")
            .IsRequired();

        builder.Property(p => p.RoomId)
            .HasColumnName("room_id");

        builder.Property(p => p.TeacherId)
            .HasColumnName("teacher_id");

        builder.Property(p => p.StartTime)
            .HasColumnName("start_time");

        builder.Property(p => p.EndTime)
            .HasColumnName("end_time");

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

        // Add unique constraint to prevent duplicate sessions for the same schedule and date
        builder.HasIndex(p => new { p.Date, p.ScheduleId })
            .IsUnique()
            .HasDatabaseName("IX_class_session_date_schedule_unique");

        // Navigation properties
        builder.HasOne(r => r.Tenant)
            .WithMany(r => r.Sessions)
            .HasForeignKey(r => r.TenantId);

        builder.HasOne(p => p.Room)
            .WithMany(p => p.Sessions)
            .HasForeignKey(p => p.RoomId);

        builder.HasOne(p => p.Teacher)
            .WithMany(p => p.Sessions)
            .HasForeignKey(p => p.TeacherId);

        builder.HasOne(p => p.Schedule)
            .WithMany(s => s.Sessions)
            .HasForeignKey(p => p.ScheduleId);
    }
}