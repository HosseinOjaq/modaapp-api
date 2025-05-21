namespace ModaApp.Domain.Entities;

public class ProductSize
{
    public int Id { get; set; }
    public string title { get; set; } = default!;

    public ICollection<ProductVariant> ProductVariants { get; set; } = default!;
}