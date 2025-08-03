namespace Tahil.Infrastructure.EntityConfigurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("room");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(p => p.Capacity)
            .HasColumnName("capacity");

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active");

        builder.Property(p => p.TenantId)
            .HasColumnName("tenant_id");

        builder.HasOne(r => r.Tenant)
            .WithMany(r => r.Rooms)
            .HasForeignKey(r => r.TenantId);
    }
}