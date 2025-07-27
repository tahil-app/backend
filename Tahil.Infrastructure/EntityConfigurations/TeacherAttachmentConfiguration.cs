namespace Tahil.Infrastructure.EntityConfigurations;

public class TeacherAttachmentConfiguration : IEntityTypeConfiguration<TeacherAttachment>
{
    public void Configure(EntityTypeBuilder<TeacherAttachment> builder)
    {
        builder.ToTable("teacher_attachment");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.TeacherId)
            .HasColumnName("teacher_id");

        builder.Property(p => p.AttachmentId)
            .HasColumnName("attachment_id");

        builder.HasOne(r => r.Teacher)
            .WithMany(r => r.TeacherAttachments)
            .HasForeignKey(r => r.TeacherId);     
    }
}