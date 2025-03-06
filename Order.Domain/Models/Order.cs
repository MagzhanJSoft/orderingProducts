using Order.Domain.Interface;

namespace Order.Domain.Models;

public class Orders
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ICollection<OrderProduct> OrderProducts { get; private set; } = new List<OrderProduct>();
    public DateTime? DeleteDate { get; private set; }

    private Orders(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    public static Orders Create()
    {
        return new Orders(Guid.NewGuid());
    }

    public Result<OrderProduct> AddProduct(Product product, int quantity)
    {
        if (product == null)
            return Result<OrderProduct>.Failure("Продукт не найден");
        if (quantity <= 0)
            return Result<OrderProduct>.Failure("Количество должно быть больше нуля");

        product.DecreaseStock(quantity);
        
        var orderProduct = new OrderProduct(this, product, quantity);
        OrderProducts.Add(orderProduct);
        return Result<OrderProduct>.Success(orderProduct);
    }
    public void SoftDelete()
    {
        DeleteDate = DateTime.UtcNow;
    }
}
