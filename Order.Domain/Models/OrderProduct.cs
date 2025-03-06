namespace Order.Domain.Models;

public class OrderProduct
{
    public Guid OrderId { get; private set; }
    public Orders Order { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public int Quantity { get; private set; }

    private OrderProduct() { }

    internal OrderProduct(Orders order, Product product, int quantity)
    {
        OrderId = order.Id;
        Order = order;
        ProductId = product.Id;
        Product = product;
        Quantity = quantity;
    }
}
