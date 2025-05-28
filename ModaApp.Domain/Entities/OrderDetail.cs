namespace ModaApp.Domain.Entities;

public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }

    public virtual Order Order { get; set; } = default!;
    public virtual Product Product { get; set; } = default!;
}