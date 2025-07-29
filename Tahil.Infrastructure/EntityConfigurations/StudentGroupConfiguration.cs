namespace Tahil.Infrastructure.EntityConfigurations;

public class StudentGroupConfiguration : IEntityTypeConfiguration<StudentGroup>
{
    public void Configure(EntityTypeBuilder<StudentGroup> builder)
    {
        builder.ToTable("student_group");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.StudentId)
            .HasColumnName("student_id");

        builder.Property(p => p.GroupId)
            .HasColumnName("group_id");

        builder.HasOne(r => r.Student)
            .WithMany(r => r.StudentGroups)
            .HasForeignKey(r => r.StudentId);     
    }
}