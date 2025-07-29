namespace Tahil.Infrastructure.EntityConfigurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("student");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Qualification)
            .HasColumnName("qualification")
            .IsRequired();

        builder.Property(p => p.UserId)
            .HasColumnName("user_id");

        builder.HasOne(r => r.User)
            .WithMany(r => r.Students)
            .HasForeignKey(r => r.UserId);

        builder.Navigation(r => r.User)
            .AutoInclude();
    }
}