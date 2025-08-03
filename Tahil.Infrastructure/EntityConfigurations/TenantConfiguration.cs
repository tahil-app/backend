namespace Tahil.Infrastructure.EntityConfigurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenant");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(p => p.Subdomain)
            .HasColumnName("sub_domain");

        builder.Property(p => p.LogoUrl)
            .HasColumnName("logo_url");

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active");
    }
}