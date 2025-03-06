using Order.Domain.Interface;

namespace Order.Domain.Models;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Stock { get; private set; }
    public DateTime? DeleteDate { get; private set; }
    public ICollection<OrderProduct> OrderProducts { get; private set; } = new List<OrderProduct>();

    private Product(Guid id, string name, int stock)
    {
        Id = id;
        Name = name;
        Stock = stock;
    }

    public static Result<Product> Create(Guid id, string name, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Product>.Failure("Название продукта не может быть пустым.");
        if (stock < 0)
            return Result<Product>.Failure("Количество не может быть отрицательным.");

        return Result<Product>.Success(new Product(id, name, stock));
    }

    public Result<bool> Update(string name, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<bool>.Failure("Название продукта не может быть пустым.");
        if (stock < 0)
            return Result<bool>.Failure("Количество не может быть отрицательным.");

        Name = name;
        Stock = stock;
        return Result<bool>.Success(true);
    }

    public void DecreaseStock(int quantity)
    {
        Stock -= quantity;
    }

    public Result<bool> IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            return Result<bool>.Failure("Количество должно быть больше нуля");
        Stock += quantity;
        return Result<bool>.Success(true);
    }

    public void SoftDelete()
    {
        DeleteDate = DateTime.UtcNow;
    }

    public void Restore()
    {
        DeleteDate = null;
    }
}

