namespace ModaApp.Domain.Entities;
public class Product
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ShippingTime { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public int DiscountId { get; set; }
    public bool Active { get; set; }



    public Discount Discount { get; set; } = default!;
    public Brand Brand { get; set; } = default!;
    public Category Category { get; set; } = default!;


    public ICollection<ShoppingItem> ShoppingItems { get; set; } = default!;
    public ICollection<ProductFile> ProductFiles { get; set; } = default!;
    public ICollection<OrderDetail> OrderDetails { get; set; } = default!;
    public ICollection<ProductRating> ProductRatings { get; set; } = default!;
    public ICollection<ProductTags> ProductTags { get; set; } = default!;
    public ICollection<ProductVariant> ProductVariants { get; set; } = default!;
    public ICollection<ProductLike> productLikes { get; set; } = default!;
}