public record OrderWithProductsDto(Guid OrderId, List<ProductInOrderDto> Products);
public record ProductInOrderDto(Guid ProductId, string ProductName, int Quantity);

