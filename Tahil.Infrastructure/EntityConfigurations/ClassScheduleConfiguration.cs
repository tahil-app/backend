namespace Tahil.Infrastructure.EntityConfigurations;

public class ClassScheduleConfiguration : IEntityTypeConfiguration<ClassSchedule>
{
    public void Configure(EntityTypeBuilder<ClassSchedule> builder)
    {
        builder.ToTable("class_schedule");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.RoomId)
            .HasColumnName("room_id")
            .IsRequired();

        builder.Property(p => p.GroupId)
            .HasColumnName("group_id")
            .IsRequired();

        builder.Property(p => p.Day)
            .HasColumnName("day");

        builder.Property(p => p.Color)
            .HasColumnName("color");

        builder.Property(p => p.StartTime)
            .HasColumnName("start_time");

        builder.Property(p => p.EndTime)
            .HasColumnName("end_time");

        builder.Property(p => p.Status)
            .HasColumnName("status");

        builder.Property(p => p.StartDate)
            .HasColumnName("start_date");

        builder.Property(p => p.EndDate)
            .HasColumnName("end_date");

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
            .WithMany(r => r.Schedules)
            .HasForeignKey(r => r.TenantId);

        builder.HasOne(p => p.Room)
            .WithMany(r => r.Schedules)
            .HasForeignKey(p => p.RoomId);

        builder.HasOne(p => p.Group)
            .WithMany(r => r.Schedules)
            .HasForeignKey(p => p.GroupId);

    }
}