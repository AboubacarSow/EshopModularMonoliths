namespace Basket.Data.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.UserName).IsUnique();

        builder.Property(s => s.UserName).IsRequired().HasMaxLength(100);
        //Setting RelationShip:
        builder
        .HasMany(s => s.Items)
        .WithOne()
        .HasForeignKey(s => s.ShoppingCartId);
    }
}

public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.ProductId).IsRequired();
        builder.Property(s => s.ProductName).IsRequired();
        builder.Property(s => s.Price).IsRequired();
        builder.Property(s => s.Quantity).IsRequired();

    }
}