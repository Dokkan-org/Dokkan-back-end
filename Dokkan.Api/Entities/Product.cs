namespace Dokkan.Api.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }= string.Empty;
    public bool IsActive { get; set; }
    public int BrandId { get; set; } 
    public int CategoryId { get; set; }
    public  decimal BasePrice { get; set; }
    public Brand Brand { get; set; } = default!;
    public Category Category { get; set; } = default!;
}
