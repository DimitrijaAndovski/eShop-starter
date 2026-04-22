using eShop.Entities;

namespace eShop.Specifications;

public class ProductSpecification:BaseSpecification<Product>
{
    public ProductSpecification(string? itemnumber, string? brand, string? sort) : base(
        c => (string.IsNullOrWhiteSpace(itemnumber) || c.ItemNumber == itemnumber) &&
        (string.IsNullOrWhiteSpace(brand) || (c.Brand.ToLower() == brand.ToLower())))
    {
        switch (sort)
        {
            case "priceAsc":
                UseOrderByAscending(c => c.Price);
                break;
            case "priceDesc":
                UseOrderByDescending(c => c.Price);
                break;
            default:
                UseOrderByAscending(c => c.ProductName);
                break;
        }
    }
    
}
