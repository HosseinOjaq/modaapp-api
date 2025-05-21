
namespace ModaApp.Domain.Entities;

public class Discount
{
    public decimal Amount { get; set; }
    public int Percent { get; set; }

    public ICollection<Product> products { get; set; } = default!;
}