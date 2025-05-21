namespace ModaApp.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    public required string Title { get; set; }

    public ICollection<ProductTags> productTags { get; set; } = default!;
}