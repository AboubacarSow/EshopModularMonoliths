using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Data.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
        public void Configure(EntityTypeBuilder<Order> builder)
        {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.CustomerId).IsRequired();

                builder.HasIndex(o => o.OrderName)
                        .IsUnique();

                builder.Property(o => o.OrderName)
                        .IsRequired()
                        .HasMaxLength(100);

                //Setting RelationShip:
                builder.HasMany(o => o.Items)
                       .WithOne()
                       .HasForeignKey(oi => oi.OrderId)
                       .OnDelete(DeleteBehavior.Cascade);


                builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
                {
                        addressBuilder.Property(ad => ad.FirstName).IsRequired().HasMaxLength(50);
                        addressBuilder.Property(ad => ad.LastName).IsRequired().HasMaxLength(50);
                        addressBuilder.Property(ad => ad.EmailAddress).IsRequired().HasMaxLength(50);
                        addressBuilder.Property(ad => ad.AddressLine).IsRequired().HasMaxLength(180);
                        addressBuilder.Property(ad => ad.Country).IsRequired().HasMaxLength(50);
                        addressBuilder.Property(ad => ad.State).IsRequired().HasMaxLength(50);
                        addressBuilder.Property(ad => ad.ZipCode).IsRequired().HasMaxLength(5);
                });
                builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
                {
                        addressBuilder.Property(ad => ad.FirstName)
                        .IsRequired()
                        .HasMaxLength(50);
                        addressBuilder.Property(ad => ad.LastName)
                            .IsRequired()
                            .HasMaxLength(50);
                        addressBuilder.Property(ad => ad.EmailAddress)
                        .IsRequired()
                        .HasMaxLength(50);
                        addressBuilder.Property(ad => ad.AddressLine)
                            .IsRequired()
                            .HasMaxLength(180);
                        addressBuilder.Property(ad => ad.Country)
                            .IsRequired()
                            .HasMaxLength(50);
                        addressBuilder.Property(ad => ad.State)
                            .IsRequired()
                            .HasMaxLength(50);
                        addressBuilder.Property(ad => ad.ZipCode)
                            .IsRequired()
                            .HasMaxLength(5);
                });
                builder.ComplexProperty(o => o.Payment, paymentBuilder =>
                {
                        paymentBuilder.Property(p => p.CardName)
                                .IsRequired()
                                .HasMaxLength(50);
                        paymentBuilder.Property(p => p.CardNumber)
                            .IsRequired()
                            .HasMaxLength(24);
                        paymentBuilder.Property(p => p.Expiration)
                        .IsRequired()
                        .HasMaxLength(10);
                        paymentBuilder.Property(p => p.CVV)
                            .IsRequired()
                            .HasMaxLength(3);
                        paymentBuilder.Property(p => p.PaymentMethod)
                                .IsRequired();
                });
        }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
                builder.HasKey(oi => oi.Id);
                builder.Property(oi => oi.OrderId).IsRequired();
                builder.Property(oi => oi.ProductId).IsRequired();
                builder.Property(oi => oi.Quantity).IsRequired();
                builder.Property(oi => oi.Price).IsRequired().HasColumnType("decimal(18,2)");
        }
}