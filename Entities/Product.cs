using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using eShop.DTOs.Products;

namespace eShop.Entities;

public record Product
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    [NotNull]
    public string ItemNumber { get; set; }
    [NotNull]
    public string ProductName { get; set; }
    [NotNull]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    [ForeignKey("SupplierId")]
    public Supplier Supplier { get; set; }

    public static implicit operator Product(GetProductDto v)
    {
        throw new NotImplementedException();
    }

}
