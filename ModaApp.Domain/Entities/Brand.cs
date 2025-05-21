namespace ModaApp.Domain.Entities;

public class Brand
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int CategoryId { get; set; }


    public Category Category { get; set; } = default!;
    public ICollection<Product> products { get; set; } = default!;
}