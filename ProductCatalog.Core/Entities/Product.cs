using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Core.Entities;

public class Product
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}