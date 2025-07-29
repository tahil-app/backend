namespace Tahil.Infrastructure.EntityConfigurations;

public class StudentAttachmentConfiguration : IEntityTypeConfiguration<StudentAttachment>
{
    public void Configure(EntityTypeBuilder<StudentAttachment> builder)
    {
        builder.ToTable("student_attachment");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.StudentId)
            .HasColumnName("student_id");

        builder.Property(p => p.AttachmentId)
            .HasColumnName("attachment_id");

        builder.Property(p => p.DisplayName)
            .HasColumnName("display_name");

        builder.HasOne(r => r.Student)
            .WithMany(r => r.StudentAttachments)
            .HasForeignKey(r => r.StudentId);     
    }
}