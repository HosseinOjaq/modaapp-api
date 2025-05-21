namespace ModaApp.Domain.Entities;

public class ShoppingItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public DateTime Created_At { get; set; }


    public Product Product { get; set; } = default!;
    public User User { get; set; } = default!;
}