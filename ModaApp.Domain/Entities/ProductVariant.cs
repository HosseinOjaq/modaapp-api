namespace ModaApp.Domain.Entities;

public class ProductVariant
{
    public int Id { get; set; }
    public int ProductSizeId { get; set; }
    public int ProductColorId { get; set; }
    public decimal Price { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }

    public ProductSize ProductSize { get; set; } = default!;
    public ProductColor ProductColor { get; set; } = default!;
    public Product Product { get; set; } = default!;
}