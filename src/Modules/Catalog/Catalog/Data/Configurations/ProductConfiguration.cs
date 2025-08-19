namespace Catalog.Data.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

        builder.Property(p => p.Category)
                .IsRequired();

        builder.Property(p => p.Description)
                .HasMaxLength(200)
                .IsRequired();

        builder.Property(p => p.ImageFile)
                .HasMaxLength(100)
                .IsRequired();

        builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();
    }
}