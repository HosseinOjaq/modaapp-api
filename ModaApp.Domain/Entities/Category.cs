using System.ComponentModel.DataAnnotations.Schema;

namespace ModaApp.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? ParentCategoryId { get; set; }

    [ForeignKey(nameof(ParentCategoryId))]
    public Category ParentCategory { get; set; } = default!;
    public ICollection<Category> ChildCategories { get; set; } = default!;
    public ICollection<Product> Products { get; set; } = default!;
    public ICollection<Brand> Brands { get; set; } = default!;
}