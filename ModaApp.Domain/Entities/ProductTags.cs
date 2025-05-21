namespace ModaApp.Domain.Entities;

public class ProductTags
{
    public int Id { get; set; }
    public int TagId { get; set; }
    public int ProductId { get; set; }


    public Tag Tags { get; set; } = default!;
    public Product Product { get; set; } = default!;
}