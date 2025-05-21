namespace ModaApp.Domain.Entities;

public class ProductFile
{
    public int ProductId { get; set; }
    public required string FileName { get; set; }
    public bool MainPicture { get; set; }

    public Product Product { get; set; } = default!;
}