namespace ModaApp.Domain.Entities;

public class ProductColor
{
    public int Id { get; set; }
    public required string Title { get; set; }

    public ICollection<ProductVariant> ProductVariants { get; set; } = default!;
}