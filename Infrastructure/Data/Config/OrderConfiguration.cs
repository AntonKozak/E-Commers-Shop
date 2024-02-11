using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Configuring ownership of ShipToAddress within Order entity.
        builder.OwnsOne(o => o.ShipToAddress, a =>
        {
            a.WithOwner();
        });
        // addeing required address for table
        builder.Navigation(a => a.ShipToAddress).IsRequired();
        
        // Configuring property Status to be converted to/from string when saving/retrieving from the database.
        builder.Property(s => s.Status)
            .HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
        );
        //to delete all order items when an order is deleted
        builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);


    }
}

