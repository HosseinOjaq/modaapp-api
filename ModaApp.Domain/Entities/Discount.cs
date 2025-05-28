
namespace ModaApp.Domain.Entities;

public class Discount
{
    public int Id { get; set; }
    public int Percent { get; set; }
    public decimal Amount { get; set; }

    public ICollection<Product> products { get; set; } = default!;
}