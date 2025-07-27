namespace Tahil.Infrastructure.EntityConfigurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("teacher");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Qualification)
            .HasColumnName("qualification")
            .IsRequired();

        builder.Property(p => p.Experience)
            .HasColumnName("experience");

        builder.Property(p => p.UserId)
            .HasColumnName("user_id");

        builder.HasOne(r => r.User)
            .WithMany(r => r.Teachers)
            .HasForeignKey(r => r.UserId);

        builder.Navigation(r => r.User)
            .AutoInclude();
    }
}