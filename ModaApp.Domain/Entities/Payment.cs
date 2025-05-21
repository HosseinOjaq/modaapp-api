using ModaApp.Domain.Enums;

namespace ModaApp.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string? Authority { get; set; }
    public int OrderId { get; set; }
    public DateTime Date { get; set; }
    public int StatusCode { get; set; }
    public PaymentStatus Status { get; set; }
    public long RefrencID { get; set; }
    public decimal Price { get; set; }

    public Order Order { get; set; } = default!;
}