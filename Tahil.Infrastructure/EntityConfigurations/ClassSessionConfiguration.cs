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

        builder.Property(p => p.OverrideRoomId)
            .HasColumnName("override_room_id");

        builder.Property(p => p.OverrideTeacherId)
            .HasColumnName("override_teacher_id");

        builder.Property(p => p.OverrideStartTime)
            .HasColumnName("override_start_time");

        builder.Property(p => p.OverrideEndTime)
            .HasColumnName("override_end_time");

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

        // Navigation properties
        builder.HasOne(r => r.Tenant)
            .WithMany(r => r.Sessions)
            .HasForeignKey(r => r.TenantId);

        builder.HasOne(p => p.OverrideRoom)
            .WithMany(p => p.Sessions)
            .HasForeignKey(p => p.OverrideRoomId);

        builder.HasOne(p => p.OverrideTeacher)
            .WithMany(p => p.Sessions)
            .HasForeignKey(p => p.OverrideTeacherId);

        builder.HasOne(p => p.Schedule)
            .WithMany(s => s.Sessions)
            .HasForeignKey(p => p.ScheduleId);
    }
}