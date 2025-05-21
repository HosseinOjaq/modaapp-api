namespace ModaApp.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal OrderSum { get; set; }
    public bool IsFinaly { get; set; }
    public DateTime CreateDate { get; set; }
    public int Code { get; set; } = 1000;

    public virtual User User { get; set; } = default!;
    public virtual ICollection<Payment> Payments { get; set; } = default!;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = default!;
}