namespace Core.Entities.OrderAggregate;

public class OrderItem: BaseEntity
{
    public OrderItem()
    {
    }

    public OrderItem( ProductItemOrdered itemOrdered, decimal price, int quantity, string pictureUrl)
    {
        ItemOrdered = itemOrdered;
        Price = price;
        Quantity = quantity;
        PictureUrl = pictureUrl;
    }

    public ProductItemOrdered ItemOrdered { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }
    
}
