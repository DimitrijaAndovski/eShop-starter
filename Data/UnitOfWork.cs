using eShop.Interfaces;
using eShop.Repositories;

namespace eShop.Data;

public class UnitOfWork(EShopContext context) : IUnitOfWork
{
    public IProductRepository ProductRepository => new ProductRepository(context);

    public ISupplierRepository SupplierRepository => new SupplierRepository(context);

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
